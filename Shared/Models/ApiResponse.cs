namespace test.Shared.Models;

public class ApiResponse
{
    public bool IsSuccess { get; init; }

    public string? TraceId { get; set; }

    public string Message { get; init; } = string.Empty;

    public Dictionary<string, string[]>? Errors { get; init; }

    public DateTimeOffset Timestamp { get; init; }

    public static ApiResponse Success(string message)
    {
        return new ApiResponse
        {
            IsSuccess = true,
            Message = message,
            Timestamp = DateTimeOffset.UtcNow
        };
    }

    //public static ApiResponse Failure(
    //    string message,
    //    Dictionary<string, string[]>? errors = null)
    //{
    //    return new ApiResponse
    //    {
    //        IsSuccess = false,
    //        Message = message,
    //        Errors = errors,
    //        TraceId = null,
    //        Timestamp = DateTimeOffset.UtcNow
    //    };
    //}
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; init; }

    public string? TraceId { get; set; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public DateTimeOffset Timestamp { get; init; }

    public static ApiResponse<T> Success(
        T data,
        string message)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}