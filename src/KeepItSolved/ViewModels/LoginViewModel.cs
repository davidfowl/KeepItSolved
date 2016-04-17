using System.ComponentModel.DataAnnotations;

namespace KeepItSolved.ViewModels
{
	public class LoginViewModel
    {
		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
    }
}
