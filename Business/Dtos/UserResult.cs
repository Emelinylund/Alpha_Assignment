using Business.Models;

namespace Business.Dtos;

public class UserResult : ServiceResult
{

    public IEnumerable<User>? Result { get; set; }
}
