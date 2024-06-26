﻿using Catering.Api.Configuration.ErrorHandling.Abstractions;

namespace Catering.Api.Configuration.ErrorHandling;

public class ErrorPublisher : IErrorPublisher
{
    private readonly Dictionary<Type, Type> _exceptionResolvers = [];

    public HttpErrorResult? Publish(Exception e)
    {
        if (e == null)
            return HttpErrorResult.DefaultResult;

        var exceptionType = e.GetType();
        var resolverExists = _exceptionResolvers.TryGetValue(exceptionType, out var resolverType);
        if (!resolverExists)
            return null;

        if (!CheckValidityOfResolverType(exceptionType, resolverType))
            return null;

        var resolverInstance = (IErrorHttpResolver)Activator.CreateInstance(resolverType);

        return resolverInstance?.Resolve(e);
    }

    public bool TryAddExceptionResolver(Type resolverType)
    {
        if (resolverType == null)
            return false;
        
        if (!resolverType.IsAssignableTo(typeof(IErrorHttpResolver)))
            return false;

        if (resolverType.BaseType == null)
            return false;
        
        var exceptionType = resolverType.BaseType.GetGenericArguments().FirstOrDefault();
        if (exceptionType == null || !exceptionType.IsAssignableTo(typeof(Exception)))
            return false;

        return _exceptionResolvers.TryAdd(exceptionType, resolverType);
    }

    public bool TryAddExceptionResolvers(IEnumerable<Type> resolverTypes)
    {
        var allSuccessful = true;
        foreach (var item in resolverTypes)
        {
            allSuccessful &= TryAddExceptionResolver(item);

            if (!allSuccessful)
            {
                _exceptionResolvers.Clear();
                break;
            }
        }

        return allSuccessful;
    }

    private bool CheckValidityOfResolverType(Type exceptionType, Type resolverType)
    {
        var isValid = true;
        var constructors = resolverType.GetConstructors();

        isValid &= constructors.Any(c => c.GetParameters().Length == 0);

        isValid &= resolverType.IsAssignableTo(typeof(ErrorHttpResolver<>).MakeGenericType(exceptionType));

        return isValid;
    }
}
