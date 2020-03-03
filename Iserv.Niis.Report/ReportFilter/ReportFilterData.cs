namespace Iserv.Niis.Report.ReportFilter
{
    public class ReportFilterData
    {
        private int? _pageNumber { get; set; }
        public int PageNumber
        {
            get
            {
                var result = _pageNumber == 0 || _pageNumber == null
                            ? 1
                            : _pageNumber;

                return (int)result;
            }
            set { _pageNumber = value; }
        }

        private int? _itemsCountPerPage { get; set; }
        public int ItemsCountPerPage
        {
            get
            {
                var result = _itemsCountPerPage == 0 || _itemsCountPerPage == null
                            ? 25
                            : _itemsCountPerPage;

                return (int)result;
            }
            set { _itemsCountPerPage = value; }
        }

    }
}
