using Business.Models;
using DataClass.Entities;

namespace Business.Extensions;

public static class MappingExtensions
{
    public static T MapTo<T>(this object source) where T : new()
    {
        if (source == null) return default!;

        T destination = new T();
        var sourceProperties = source.GetType().GetProperties();
        var destinationProperties = typeof(T).GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);

            if (destinationProperty != null && destinationProperty.CanWrite)
            {
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        return destination;
    }

    public static List<T> MapTo<T>(this IEnumerable<object> sourceList) where T : new()
    {
        return sourceList.Select(item => item.MapTo<T>()).ToList();
    }
}
public static class ProjectMapperExtensions
{
    public static ProjectEntity MapTo(this AddProjectForm form)
    {
        return new ProjectEntity
        {
            ProjectName = form.ProjectName,
            Description = form.Description,
            StartDate = form.StartDate ?? DateTime.MinValue,
            EndDate = form.EndDate,
            Budget = form.Budget ?? 0,
            ClientId = form.ClientId.ToString(),
            StatusId = form.Status,
            Created = DateTime.Now,
            // Här kan du hantera filuppladdning om det behövs också
        };
    }
}