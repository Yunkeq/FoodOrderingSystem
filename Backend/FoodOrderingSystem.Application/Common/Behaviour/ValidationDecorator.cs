using FluentValidation;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Common.Behaviour;

public sealed class ValidationDecorator<TCommand, TResponse>
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _inner;
    private readonly IValidator<TCommand>? _validator;

    public ValidationDecorator(
        ICommandHandler<TCommand, TResponse> inner,
        IValidator<TCommand>? validator = null)
    {
        _inner = inner;
        _validator = validator;
    }

    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        if (_validator is not null)
        {
            var validation = await _validator.ValidateAsync(command, cancellationToken);

            if (!validation.IsValid)
            {
                var message = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));
                return Result<TResponse>.Failure(new Error(ErrorCode.Validation, message));
            }
        }

        return await _inner.Handle(command, cancellationToken);
    }
}

public sealed class ValidationDecorator<TCommand>
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner;
    private readonly IValidator<TCommand>? _validator;

    public ValidationDecorator(
        ICommandHandler<TCommand> inner,
        IValidator<TCommand>? validator = null)
    {
        _inner = inner;
        _validator = validator;
    }

    public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
    {
        if (_validator is not null)
        {
            var validation = await _validator.ValidateAsync(command, cancellationToken);

            if (!validation.IsValid)
            {
                var message = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));
                return Result.Failure(new Error(ErrorCode.Validation, message));
            }
        }

        return await _inner.Handle(command, cancellationToken);
    }
}
