namespace Algorithmix.Models.Account
{
    public class ResetPasswordPayload
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
