namespace ApiProj.Wrappers
{
    public class PagedResponse<T> : Response<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; } = 0;
        public Uri Next {  get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, Uri next)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Next = next;
        }
    }
}
