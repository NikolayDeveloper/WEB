using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Business.Implementations
{
    public class NumberGenerator : INumberGenerator
    {
        public const string ContractCode = "ContractNum";

        private const string ProtectionDocTypeInventionCode = "B";
        private const string ProtectionDocTypeUsefulModelCode = "U";
        private const string ProtectionDocTypeIndustrialSampleCode = "S2";
        private const string ProtectionDocTypeNameOfOriginCode = "PN";
        private const string ProtectionDocTypeSelectionAchieveCode = "SA";

        private static readonly string[] FormationCodes =
            {"NMPT02.1", "TMI02.1", "DK02.1", "AP01.1", "B02.1_IN", "U02.1", "PO02.1", "SA02.1", "TM02.1", "B02.1"};

        private readonly NiisWebContext _context;

        public NumberGenerator(NiisWebContext context)
        {
            _context = context;
        }

        public void GenerateBarcode(IHaveBarcode haveBarcode)
        {
            haveBarcode.Barcode = Generate("Barcode");
        }

        public async Task GenerateRequestNum(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (string.IsNullOrEmpty(request.RequestNum) == false)
            {
                return;
            }

            var currentStageCode = _context.DicRouteStages.Single(s => s.Id == request.CurrentWorkflow.CurrentStageId).Code;
            if (!FormationCodes.Contains(currentStageCode))
            {
                throw new Exception("Unsuitable stage!");
            }

            var code = request.ProtectionDocType.Code;
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException($"Protection doc type code");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    request.RequestNum = GetRequestNumber("RequestNum_" + code, code, request.SelectionAchieveType?.Code);
                    request.RequestDate = DateTimeOffset.Now;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Could not generate number: {ex.Message}");
                }

            }
        }

        public void GenerateIncomingNum(Request request)
        {
            if (request.IncomingNumber != null)
            {
                return;
            }

            request.IncomingNumber = $"{DateTime.Now.Year}-{Generate("RequestIncomingNumber")}";
        }

        /// <summary>
        /// Генерация входящего номера для документа
        /// </summary>
        /// <param name="document"></param>
        public void GenerateIncomingNum(Document document)
        {
            var department = _context.DicDepartments
                .Include(d => d.Division)
                .Single(d => d.Id == document.DepartmentId);

            if (department.Division.Code.Equals("000001"))
            {
                if (document.IncomingNumber != null)
                {
                    return;
                }

                document.IncomingNumber =
                    $"{DateTime.Now.Year}-{Generate("DocumentIncomingNumber" + department.Division.Code):D5}";
            }
            else
            {
                if (document.IncomingNumberFilial != null)
                {
                    return;
                }

                document.IncomingNumberFilial =
                    $"{Generate("DocumentIncomingNumberFilial" + department.Division.Code):D5}-{department.Division.IncomingNumberCode}";
            }
        }

        /// <summary>
        /// Генерация входящего номера для договора
        /// </summary>
        /// <param name="contract"></param>
        public void GenerateIncomingNum(Contract contract)
        {
            var division = _context.DicDivisions.FirstOrDefault(d => d.Id == contract.DivisionId);
            var commonDivision = _context.DicDivisions.FirstOrDefault(d => d.Code == DicDivisionCodes.RGP_NIIS);
            if (division == null)
                division = commonDivision;


            contract.IncomingNumber =
                    $"{DateTime.Now.Year}-{Generate("DocumentIncomingNumber" + division.Code):D5}";

        }

        /// <summary>
        /// Генерация исходящего номера для документа
        /// </summary>
        /// <param name="document"></param>
        public void GenerateOutgoingNum(Document document)
        {
            var request = _context.Requests
                .Include(r => r.Division)
                .FirstOrDefault(r => r.Documents.Any(d => d.DocumentId == document.Id));
            var contract = _context.Contracts
                .FirstOrDefault(c => c.Documents.Any(d => d.DocumentId == document.Id));
            var protectionDoc = _context.ProtectionDocs
                .FirstOrDefault(p => p.Documents.Any(d => d.DocumentId == document.Id));

            if (document.OutgoingNumber != null)
            {
                return;
            }

            if (protectionDoc != null)
            {
                document.OutgoingNumber = GenerateOutgoingNumberForMainFilial();
                return;
            }

            if (contract != null)
            {
                document.OutgoingNumber = GenerateOutgoingNumberForMainFilial();
                return;
            }

            if (request == null)
            {
                return;
            }

            if (request.Division == null)
            {
                document.OutgoingNumber = GenerateOutgoingNumberForMainFilial();
                return;
            }

            var divisionCode = request.Division.Code;

            document.OutgoingNumber = divisionCode.Equals("000001")
                ? GenerateOutgoingNumberForMainFilial()
                : $"{Generate("DocumentOutgoingNumberFilial" + divisionCode):D5}-{request.Division.IncomingNumberCode}";
        }

        public void GenerateNumForRegisters(Document document)
        {
            if (document.DocumentNum != null)
            {
                return;
            }

            var code = _context.DicDocumentTypes
                .Where(d => d.ExternalId == document.TypeId && !d.IsDeleted)
                .Select(d => d.Code)
                .SingleOrDefault() ??
                _context.DicDocumentTypes
                .Where(d => d.Id == document.TypeId && !d.IsDeleted)
                .Select(d => d.Code)
                .SingleOrDefault();
          

            if (code.Equals(string.Empty))
           
            {
                throw new Exception();
            }

            code += DateTime.Now.Year;

            document.DocumentNum = Generate(code).ToString();

        } 
        public int Generate(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException($"Request type code");
            }

            return GetNextCount(code);
        }

        private string GenerateOutgoingNumberForMainFilial()
        {
            return $"{DateTime.Now.Year}-{Generate("DocumentOutgoingNumber" + "000001"):D5}";
        }

        public string GetRequestNumber(string code, string protectionDocTypeCode, string selectionAchieveType)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (string.IsNullOrEmpty(protectionDocTypeCode))
            {
                throw new ArgumentNullException(nameof(protectionDocTypeCode));
            }

            var nextCount = GetNextCount(code);
            switch (protectionDocTypeCode)
            {
                case ProtectionDocTypeInventionCode:
                    return GetNumberForInventionUsefulModel(nextCount, "1");
                case ProtectionDocTypeIndustrialSampleCode:
                    return GetNumberForIndustrialSample(nextCount);
                case ProtectionDocTypeUsefulModelCode:
                    return GetNumberForInventionUsefulModel(nextCount, "2");
                case ProtectionDocTypeNameOfOriginCode:
                    var numberStr = nextCount.ToString();
                    var preparedNum = numberStr.Length == 1 ? $"0{nextCount}" : numberStr;
                    return $"{preparedNum}-1";
                case ProtectionDocTypeSelectionAchieveCode:
                    return GetNumberForSelectionAchieve(nextCount, selectionAchieveType);
                default:
                    return nextCount.ToString();
            }
        }

        private int GetNextCount(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var currentCounter = _context.SystemCounter.SingleOrDefault(c => c.Code.Equals(code));
            SystemCounter nextCounter;

            if (currentCounter != null)
            {
                currentCounter.Count = ++currentCounter.Count;
                nextCounter = _context.Update(currentCounter).Entity;
            }
            else
            {
                nextCounter = _context.Add(new SystemCounter { Code = code, Count = 1 }).Entity;
            }

            return nextCounter.Count;
        }

        private string GetNumberForInventionUsefulModel(int number, string specialValue)
        {
            const int requiredLengthNum = 4;
            var strNum = number.ToString();
            if (strNum.Length < requiredLengthNum)
            {
                strNum = strNum.Length == 3
                    ? $"0{strNum}" : strNum.Length == 2
                    ? $"00{strNum}" : $"000{strNum}";
            }

            return $"{DateTime.Now.Year}/{strNum}.{specialValue}";
        }

        private string GetNumberForIndustrialSample(int number)
        {
            const int requiredLengthNum = 3;
            var strNum = number.ToString();
            if (strNum.Length < requiredLengthNum)
            {
                strNum = strNum.Length == 2 ? $"0{strNum}" : $"00{strNum}";
            }

            return $"{DateTime.Now.Year}{strNum}.3";
        }

        private string GetNumberForSelectionAchieve(int number, string selectionAchieveType)
        {
            const int requiredLengthNum = 3;
            var isCrop = string.Equals(selectionAchieveType, DicSelectionAchieveTypeCodes.Agricultural);
            var specialValue = isCrop ? "4" : "5";
            var strNum = number.ToString();
            if (strNum.Length < requiredLengthNum)
            {
                strNum = strNum.Length == 2 ? $"0{strNum}" : $"00{strNum}";
            }

            return $"{DateTime.Now.Year}/{strNum}.{specialValue}";
        }

        public void GenerateGosNumber(Contract contract)
        {
            if (contract.GosNumber != null)
            {
                return;
            }
            var dkCode = contract.RequestsForProtectionDoc.FirstOrDefault().Request.ProtectionDocType.DkCode;
            contract.GosNumber = $"{dkCode} {DateTime.Now.Year} {Generate("GosNumber")}/{contract.Type?.Code.Split('_').LastOrDefault()}-{contract.Category?.Code}";
        }

        public void GenerateProtectionDocGosNumber(int[] ids)
        {
            var protectionDocsGroup = _context.ProtectionDocs.Include(pd => pd.Type)
                .Include(pd => pd.IpcProtectionDocs)
                .Where(pd => ids.Contains(pd.Id))
                .GroupBy(pd => pd.Type.Code)
                .Select(pd => pd);
            foreach (var protectionDocs in protectionDocsGroup)
            {
                switch (protectionDocs.Key)
                {
                    case "S2":
                    case "TM":
                    case "SA":
                        var orderedProtectionDocs = protectionDocs.OrderBy(pds => pds.RegNumber);
                        foreach (var protectionDoc in orderedProtectionDocs)
                        {
                            if(!string.IsNullOrEmpty(protectionDoc.GosNumber)) continue;
                            switch (protectionDoc.Type.Code)
                            {
                                case "S2":
                                    protectionDoc.GosNumber = $"{DateTimeOffset.Now.Year}/{Generate("PDS2"):D3}.3";
                                    break;
                                case "TM":
                                    protectionDoc.GosNumber = $"{Generate("PDTM"):D3}";
                                    break;
                                case "SA":
                                    protectionDoc.GosNumber = $"{DateTimeOffset.Now.Year}/{Generate("PDSA"):D3}.4";
                                    break;
                            }
                            protectionDoc.GosDate = DateTimeOffset.Now;
                        }
                        break;
                    case "B":
                    case "U":
                        var areIpcCodesTheSame = protectionDocs.GroupBy(protectionDoc =>
                                protectionDoc.IpcProtectionDocs.FirstOrDefault()?.Ipc.Code ?? string.Empty)
                            .Select(pd => pd).Count() == 1;
                        var orderedMpcProtectionDocs = protectionDocs.OrderBy(pds =>
                            areIpcCodesTheSame
                                ? pds.RegNumber
                                : pds.IpcProtectionDocs.FirstOrDefault()?.Ipc?.Code ?? pds.RegNumber);
                        foreach (var protectionDoc in orderedMpcProtectionDocs)
                        {
                            if (!string.IsNullOrEmpty(protectionDoc.GosNumber)) continue;
                            switch (protectionDoc.Type.Code)
                            {
                                case "B":
                                    protectionDoc.GosNumber = $"{DateTimeOffset.Now.Year}/{Generate("PDB"):D3}.1";
                                    break;
                                case "U":
                                    protectionDoc.GosNumber = $"{DateTimeOffset.Now.Year}/{Generate("PDU"):D3}.2";
                                    break;
                            }
                            protectionDoc.GosDate = DateTimeOffset.Now;
                        }
                        break;
                }
            }
            _context.SaveChanges();
        }
    }
}