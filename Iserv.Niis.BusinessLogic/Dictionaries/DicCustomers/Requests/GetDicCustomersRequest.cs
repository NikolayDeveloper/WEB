namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCustomers.Requests
{
    public class GetDicCustomersRequest
    {
        public int Id { get; set; }
        public string Xin { get; set; }
        public string Name { get; set; }
        public string RegNumber { get; set; }
        public bool IsPatentAttorney { get; set; }
        public int CustomerTypeId { get; set; }

        public bool HasXin => string.IsNullOrEmpty(Xin) == false;
        public bool HasName => string.IsNullOrEmpty(Name) == false;
        public bool HasRegNumer => string.IsNullOrEmpty(RegNumber) == false;
        public bool HasId => Id != 0;
        public bool HasCustomerTypeId => CustomerTypeId != 0;

        public static GetDicCustomersRequest ConstructFromQueryStringParameters(string queryStringParams)
        {
            var splitedParams = queryStringParams.Split(';');
            int id = 0;
            int.TryParse(splitedParams[0], out id);
            int customerTypeId = 0;
            int.TryParse(splitedParams[5], out customerTypeId);
            return new GetDicCustomersRequest
            {
                Id = id,
                Xin = splitedParams[1],
                Name = splitedParams[2],
                IsPatentAttorney = bool.Parse(splitedParams[3]),
                RegNumber = splitedParams[4],
                CustomerTypeId = customerTypeId
            };
        }
    }
}