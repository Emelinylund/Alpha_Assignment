using System.ComponentModel.DataAnnotations;

namespace Alpha_Assignment.Models;

public class RegisterFormModel
{
    [Display(Name = "First Name" , Prompt = "Your first name")]
    [Required(ErrorMessage = "You must enter your first name.")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Your last name")]
    [Required(ErrorMessage = "You must enter your last name.")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "You must enter your email address.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage= "You must enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "You must enter a password.")]
    [RegularExpression("^(?=.*[a-ö])(?=.*[A-Ö])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "You must enter a strong password.")]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Your password do not match.")]
    [Required(ErrorMessage = "You must confirm your password.")]
    public string ConfirmPassword { get; set; } = null!;

    [Display(Name = "Terms and Conditions", Prompt = "I accept terms and conditions.")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions.")]
    public bool AcceptTerms { get; set; }

}
