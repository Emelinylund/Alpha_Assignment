using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class EditProjectForm
{
    public string Id { get; set; } = null!;

    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    [Display(Name = "Project Name", Prompt = "Project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Requierd")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Requierd")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Requierd")]
    public string Description { get; set; } = null!;

    [Display(Name = "Start Date", Prompt = "Enter start date")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "Enter end date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "Enter Budget")]
    [DataType(DataType.Currency)]
    public decimal? Budget { get; set; }

    [Display(Name = "Status", Prompt = "Enter Status")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Requierd")]
    public string Status { get; set; } = null!;

}
