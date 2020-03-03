using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Model.Models.BibliographicData;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Changes
{
    public class GetChangeTypeOptionsQuery: BaseQuery
    {
        public List<ChangeTypeOptionDto> Execute()
        {
            return new List<ChangeTypeOptionDto>
            {
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение изменений в адрес переписки",
                    Code = "AddrAddr",
                    StageCode = RouteStageCodes.TZ_03_3_8,
                    Types = new[]
                    {
                        ChangeType.AddresseeAddress,
                        ChangeType.AddresseeAddressEn,
                        ChangeType.AddresseeAddressKz
                    }
                },
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение изменений в наименование заявителя",
                    Code = "DeclName",
                    StageCode = RouteStageCodes.TZ_03_3_8,
                    Types = new[]
                    {
                        ChangeType.DeclarantName,
                        ChangeType.DeclarantNameEn,
                        ChangeType.DeclarantNameKz
                    }
                },
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение изменений в адрес заявителя",
                    Code = "DeclAddr",
                    StageCode = RouteStageCodes.TZ_03_3_8,
                    Types = new[]
                    {
                        ChangeType.DeclarantAddress,
                        ChangeType.DeclarantAddressEn,
                        ChangeType.DeclarantAddressKz
                    }
                },
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение изменений в заявленное обозначение",
                    Code = "Image",
                    StageCode = RouteStageCodes.TZSecondFullExpertize,
                    Types = new[]
                    {
                        ChangeType.Image,
                    }
                },
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение изменений в перечень товаров",
                    Code = "Icgs",
                    StageCode = RouteStageCodes.TZSecondFullExpertize,
                    Types = new[]
                    {
                        ChangeType.Icgs,
                    }
                },
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение изменений в представителя заявителя",
                    Code = "Attorney",
                    StageCode = RouteStageCodes.TZ_03_3_8,
                    Types = new[]
                    {
                        ChangeType.Declarant,
                        ChangeType.Addressee,
                        ChangeType.PatentAttorney
                    }
                },
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение уточнений, корректировок и дополнений в заявку",
                    Code = "FullExp",
                    StageCode = RouteStageCodes.TZSecondFullExpertize,
                    Types = new[]
                    {
                        ChangeType.Everything,
                    }
                },
                new ChangeTypeOptionDto
                {
                    NameRu = "Внесение изменений в части исправления ошибки (ошибок) в бланке заявления",
                    Code = "FullGosr",
                    StageCode = RouteStageCodes.TZ_03_3_8,
                    Types = new[]
                    {
                        ChangeType.Everything,
                    }
                },
            };
        }
    }
}
