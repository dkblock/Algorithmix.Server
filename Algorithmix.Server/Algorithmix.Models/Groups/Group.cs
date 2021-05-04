namespace Algorithmix.Models.Groups
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailableForRegister { get; set; }
        public int UsersCount { get; set; }
    }
}
