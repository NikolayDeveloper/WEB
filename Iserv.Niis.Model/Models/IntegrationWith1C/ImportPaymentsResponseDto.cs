namespace Iserv.Niis.Model.Models.IntegrationWith1C
{
    public class ImportPaymentsResponseDto
    {
        public int ImportedNumber { get; set; }
        public bool Error { get; set; }
        public ImportPaymentsErrorType? ErrorType { get; set; }
    }
}