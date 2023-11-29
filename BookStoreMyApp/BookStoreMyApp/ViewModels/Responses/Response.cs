namespace BookStoreMyApp.Responses
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data ,string? error="")
        {
            Succeeded = error=="";
            Message = string.Empty;
            Errors = error;
            Data = data;
        }
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string? Errors { get; set; }
        public string Message { get; set; }

    }
}
