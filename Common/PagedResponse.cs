namespace Common 
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => PageNumber > 0;
        public bool HasNext => PageNumber < TotalPages - 1;

        public PagedResponse(List<T> locations, int pageNumber, int pageSize, int totalCount)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.Data = locations;
            this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
