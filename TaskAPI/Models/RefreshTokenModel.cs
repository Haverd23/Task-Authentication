namespace TaskAPI.Models
{
    public class RefreshTokenModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
