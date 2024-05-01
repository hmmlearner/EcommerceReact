namespace EcommerceReact.Server.Models
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool? Success { get; set; }
    }
}
