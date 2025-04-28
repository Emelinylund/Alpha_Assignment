using Business.Models;

namespace Business.Dtos;

public class StatusResult<T> : ServiceResult
{
    
    public T? Result { get; set; }
}

public class StatusResult : ServiceResult
{

    
}
