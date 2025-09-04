namespace DocTask.Core.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message = "Bad Request") : base(message)
    {
        StatusCode = 400;
    }
}

//  Bị cấm truy nhập - 401
public class UnauthorizedException : BaseException
{
    public UnauthorizedException(string message = "Unauthorized") : base(message)
    {
        StatusCode = 401;
    }
}

// không có quyền truy nhập - 403
public class ForbiddenException : BaseException
{
    public ForbiddenException(string message = "Forbidden") : base(message)
    {
        StatusCode = 403;
    }
}

// không tìm thấy - 404
public class NotFoundException : BaseException
{
    public NotFoundException(string message = "Not Found") : base(message)
    {
        StatusCode = 404;
    }
}

// Xung đột - 405
public class ConflictException : BaseException
{
    public ConflictException(string message = "Conflict") : base(message)
    {
        StatusCode = 409;
    }
}


// quá nhiều yêu cầu - 429
public class TooManyRequestsException : BaseException
{
    public TooManyRequestsException(string message = "Too Many Requests") : base(message)
    {
        StatusCode = 429;
    }
}


// lỗi máy chủ nội bộ - 500
public class InternalServerErrorException : BaseException
{
    public InternalServerErrorException(string message = "Internal Server Error") : base(message)
    {
        StatusCode = 500;
    }
}

// dịch vụ không khả dụng - 503
public class ServiceUnavailableException : BaseException
{
    public ServiceUnavailableException(string message = "Service Unavailable") : base(message)
    {
        StatusCode = 503;
    }
}