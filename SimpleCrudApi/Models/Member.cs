using System.Reflection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace SimpleCrudApi.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberAddress { get; set; }
        public int MemberTypeId { get; set; }
        public string MemberPhoto { get; set; }
        public byte[] MemberSignature { get; set; }
        public bool IsDeleted { get; set; } = false;
        public MemberType MemberType { get; set; }
    }

}



