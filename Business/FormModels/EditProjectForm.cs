using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Business.Models;

public class EditProjectForm
{
    [Required]
    public string Id { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    [Display(Name = "Project Name")]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name")]
    [Required(ErrorMessage = "Required")]
    public string ClientId { get; set; } = null!;

    public IEnumerable<Client> Clients { get; set; } = [];

    [Display(Name = "Description")]
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
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public int StatusId { get; set; }

    public IEnumerable<Status> Statuses { get; set; } = [];
}
