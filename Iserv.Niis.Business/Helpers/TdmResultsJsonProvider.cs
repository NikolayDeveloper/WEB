using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.Model.Models.ExpertSearch;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Iserv.Niis.Business.Helpers
{
    public static class TdmResultsJsonProvider
    {
	    public static async Task<ICollection<TrademarkDto>> ImageSearchResultsDeserializer(string imageResponse, string phoneticResponse, string semanticResponse, ICollection<TrademarkDto> collection, IMapper mapper, NiisWebContext context, IFileStorage fileStorage)
	    {
		    if (imageResponse != "")
		    {
			    var results = JsonConvert.DeserializeObject<dynamic>(imageResponse);
			    foreach (var result in results.results)
			    {
				    int.TryParse(result.barcode.ToString(), out int supervisorBarcode);
				    var protDoc =
					    context.ProtectionDocs
						    .Include(pd => pd.EarlyRegs)
						    .Include(p => p.Request).ThenInclude(er => er.EarlyRegs)
						    .Include(pd => pd.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
						    .Include(pd => pd.Icfems).ThenInclude(i => i.DicIcfem)
						    .FirstOrDefault(pd => pd.Barcode == supervisorBarcode);

				    if (protDoc != null)
				    {
					    var tdmDet = mapper.Map<TrademarkDto>(protDoc);
					    //double.TryParse(result.similarity, out double similarityValue);
					    tdmDet.ImageSimilarity = result.similarity;

					    byte[] responseImage = null;
						//responseImage = context.Requests.FirstOrDefault(r => r.Id == tdmDet.Id)?.PreviewImage; ??
						//   await fileStorage.GetAsync("images", $"{tdmDet.Barcode}.jpg");
						responseImage = await fileStorage.GetAsync("images", $"{tdmDet.Barcode}.jpg");
						tdmDet.PreviewImage =
						    responseImage != null ? $"data:image/png;base64,{Convert.ToBase64String(responseImage)}" : null;
					    collection.Add(tdmDet);
				    }
			    }
		    }
		    if (phoneticResponse != "")
		    {
			    var searchByPhoneticResponse = JsonConvert.DeserializeObject<dynamic>(phoneticResponse);
			    foreach (var result in searchByPhoneticResponse.results)
			    {
				    int.TryParse(result.barcode.ToString(), out int supervisorBarcode);
				    var protDoc =
					    context.ProtectionDocs
						    .Include(pd => pd.EarlyRegs)
						    .Include(p => p.Request).ThenInclude(er => er.EarlyRegs)
						    .Include(pd => pd.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
						    .Include(pd => pd.Icfems).ThenInclude(i => i.DicIcfem)
						    .FirstOrDefault(pd => pd.Barcode == supervisorBarcode);

				    if (protDoc != null)
				    {
					    var tdmDet = mapper.Map<TrademarkDto>(protDoc);
					    tdmDet.PhonSimilarity = result.similarity;

					    byte[] responseImage = null;
					    responseImage = await fileStorage.GetAsync("images", $"{tdmDet.Barcode}.jpg");

					    tdmDet.PreviewImage =
						    responseImage != null ? $"data:image/png;base64,{Convert.ToBase64String(responseImage)}" : null;
					    collection.Add(tdmDet);
				    }
			    }
		    }

	        if (semanticResponse == "") return collection;
	        {
	            var searchBySemanticResponse = JsonConvert.DeserializeObject<dynamic>(semanticResponse);
	            foreach (var result in searchBySemanticResponse.results)
	            {
	                int.TryParse(result.barcode.ToString(), out int supervisorBarcode);
	                var protDoc =
	                    context.ProtectionDocs
	                        .Include(pd => pd.EarlyRegs)
	                        .Include(p => p.Request).ThenInclude(er => er.EarlyRegs)
	                        .Include(pd => pd.IcgsProtectionDocs).ThenInclude(i => i.Icgs)
	                        .Include(pd => pd.Icfems).ThenInclude(i => i.DicIcfem)
	                        .FirstOrDefault(pd => pd.Barcode == supervisorBarcode);

	                if (protDoc == null) continue;
	                var tdmDet = mapper.Map<TrademarkDto>(protDoc);
	                tdmDet.SemSimilarity = result.similarity;

	                byte[] responseImage = null;
	                responseImage = await fileStorage.GetAsync("images", $"{tdmDet.Barcode}.jpg");

	                tdmDet.PreviewImage =
	                    responseImage != null ? $"data:image/png;base64,{Convert.ToBase64String(responseImage)}" : null;
	                collection.Add(tdmDet);
	            }
	        }
	        return collection;
		}
    }
}
