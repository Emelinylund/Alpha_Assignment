using System.ComponentModel.DataAnnotations;

namespace Alpha_Assignment.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ProjectName { get; set; } = null!;

    [Required]
    public string ClientName { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public decimal Budget { get; set; }
}
