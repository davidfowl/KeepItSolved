using System.ComponentModel.DataAnnotations;

namespace KeepItSolved.ViewModels
{
	public class RegisterViewModel
    {
		[Required]
		[StringLength(20, ErrorMessage = "Login must have a minimum length of 3 and a maximum length of 20.",
			MinimumLength = 3)]
		public string Login { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(256, ErrorMessage = "Password must have a minimum length of 6 and a maximum length of 256.",
			 MinimumLength = 6)]
		public string Password { get; set; }
    }
}
