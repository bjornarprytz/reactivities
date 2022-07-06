using LanguageExt;
using MediatR;
using Unit = MediatR.Unit;

namespace Application.Core;

public interface IRequestWrapper<T> : IRequest<Either<Error, T>> { }
public interface IRequestWrapper : IRequest<Either<Error, Unit>> { }

public interface IRequestHandlerWrapper<in TRequest, TResponse> : IRequestHandler<TRequest, Either<Error, TResponse>>
    where TRequest : IRequestWrapper<TResponse>
{ }

public interface IRequestHandlerWrapper<in TRequest> : IRequestHandler<TRequest, Either<Error, Unit>>
    where TRequest : IRequestWrapper
{ }