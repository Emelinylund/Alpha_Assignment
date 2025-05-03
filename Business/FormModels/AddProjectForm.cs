using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.Models;

public class AddProjectForm
{
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    [Display(Name = "Project Name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Select a client")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientId { get; set; } = null!;


    public IEnumerable<Client> Clients { get; set; } = [];

    [Display(Name = "Description", Prompt = "Enter description")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
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
    [Required(ErrorMessage = "Required")]
    public int StatusId { get; set; } 

    public IEnumerable<Status> Statuses { get; set; } = [];


}
