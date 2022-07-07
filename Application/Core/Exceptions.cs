namespace Application.Core;

public record AppException(int StatusCode, string Message, string? Details=default);