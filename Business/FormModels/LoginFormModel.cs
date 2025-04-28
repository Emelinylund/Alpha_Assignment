namespace Alpha_Assignment.Models;
using System.ComponentModel.DataAnnotations;


public class LoginFormModel
{

    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "You must enter your email address.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "You must enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "You must enter a password.")]
    [RegularExpression("^(?=.*[a-ö])(?=.*[A-Ö])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "You must enter a strong password.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "You must accept the terms and conditions.")]
    public bool RememberMe { get; set; }
}
