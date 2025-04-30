using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Business.Dtos;

namespace Business.Models;

public class EditProjectForm
{
    [Required]
    public string Id { get; set; } = null!;

    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    [Display(Name = "Project Name", Prompt = "Project name")]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client", Prompt = "Choose a client")]
    [Required(ErrorMessage = "Required")]
    public string ClientId { get; set; } = null!;

    public IEnumerable<Client> Clients { get; set; } = [];

    [Display(Name = "Description", Prompt = "Type something")]
    [Required(ErrorMessage = "Required")]
    public string Description { get; set; } = null!;

    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget")]
    [DataType(DataType.Currency)]
    public decimal? Budget { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "Required")]
    public int Status { get; set; }

    public IEnumerable<Status> Statuses { get; set; } = [];
}
