namespace FinalProject.Domain.Dtos
{
    public class SeleniumLodDto
    {
        public string Message { get; set; }
        public DateTimeOffset  SentOn { get; set; }
        public SeleniumLodDto(string message)
        {
                Message = message;
               SentOn = DateTimeOffset.Now;
        }
    }
}
