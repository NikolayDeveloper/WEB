using Iserv.Niis.Model.Models.Material;

namespace Iserv.Niis.Documents.Abstractions
{
    public interface ITemplateUserInputChecker
    {
        bool GetConfig(string templateCode, out UserInputConfigDto fieldsConfig);
    }
}
