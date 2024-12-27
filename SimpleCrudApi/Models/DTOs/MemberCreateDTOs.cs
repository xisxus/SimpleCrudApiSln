namespace SimpleCrudApi.Models.DTOs
{
    public class MemberCreateDTOs
    {
        public int? MemberId { get; set; }
        public string? MemberName { get; set; }
        public string? MemberAddress { get; set; }
        public int? MemberTypeId { get; set; }
        public IFormFile? MemberPhoto { get; set; }
        public IFormFile? MemberSignature { get; set; }
        
    }
}
