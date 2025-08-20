namespace Core.DTOs
{
    public class ValidationErrorDTO
    {
        public string? Field { get; set; }
        public List<string>? Message { get; set; }
    }
}
