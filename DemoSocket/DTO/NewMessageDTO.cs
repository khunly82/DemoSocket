namespace DemoSocket.DTO
{
    public class NewMessageDTO
    {
        public NewMessageDTO(string user, string content, DateTime created)
        {
            User = user;
            Content = content;
            Created = created;
        }

        public string User { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
