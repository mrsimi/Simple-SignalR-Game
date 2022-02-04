namespace server.Models
{
    public class AppData
    {
        public int Id { get; set; }
        public long GuessedNumber { get; set; }
        public string? CorrectUser {get; set;}
        public bool IsCompleted {get; set;} = false;
        public DateTime DateCreated {get; set;}
    }
}