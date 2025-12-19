namespace ApiProj.Wrappers
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Message { get; set;  }

        public Response() { }
        public Response(T data)
        {
            Succeeded = true;
            Data = data;
            Errors = Enumerable.Empty<string>();
            Message = string.Empty;
        }
    }
}
