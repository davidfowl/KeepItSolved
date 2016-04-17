namespace KeepItSolved.Models
{
	public class Flashcard
    {
		public int Id { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public string Category { get; set; }
		public string UserName { get; set; }
    }
}
