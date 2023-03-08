namespace Common
{
    public class PagingFilteringParameters
    {
        public PagingFilteringParameters() 
        {
            PageSize= 10;
            PageNumber = 0;
        }

        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 0;
        private int _pageSize = 10;
        public string OrderBy { get; set; } = "Name";
        public bool OrderDescending { get; set; } = false;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}

