namespace KimiStudio.Bangumi.Api.Models
{
    public class DefaultResult
    {
        public int Code { get; set; }
        public string Error { get; set; }
        public string Request { get; set; }

        public bool IsSuccess()
        {
            return Code == 200;
        }
    }
}
