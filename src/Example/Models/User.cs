namespace Example.Models
{
    public class User
    {
        public User()
        {
        }

        public int Id { get; set; }
        public string YotiId { get; set; }
        public byte[] Photo { get; set; }
        public string Base64Photo { get; set; }
        public string PhoneNumber { get; set; }
    }
}