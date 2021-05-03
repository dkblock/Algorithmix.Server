namespace Algorithmix.Models.Account
{
    public class RegisterPayload
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int GroupId { get; set; }
    }
}
