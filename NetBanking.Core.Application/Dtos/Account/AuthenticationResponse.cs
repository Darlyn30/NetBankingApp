using NetBanking.Core.Application.Dtos.Account.ResponsesCommon;

namespace NetBanking.Core.Application.Dtos.Account
{
    public class AuthenticationResponse : Responses
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string IdentificationNumber { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
