using System.ComponentModel.DataAnnotations;

namespace KeepItSolved.ViewModels
{
	public class FlashcardViewModel
    {


		public int Id { get; set; }

		[Required]
		public string Question { get; set; }
		public string Answer { get; set; } = "Not specified";
		public string Category { get; set; } = "Not specified";
	}
}
