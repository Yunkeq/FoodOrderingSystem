using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Common.Behaviour;

public sealed class TransactionDecorator<TCommand, TResponse>
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly ICommandHandler<TCommand, TResponse> _inner;
    private readonly IApplicationDbContext _dbContext;

    public TransactionDecorator(ICommandHandler<TCommand, TResponse> inner, IApplicationDbContext dbContext)
    {
        _inner = inner;
        _dbContext = dbContext;
    }

    public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await _inner.Handle(command, cancellationToken);

            if (!result.IsSuccess)
            {
                await transaction.RollbackAsync(cancellationToken);
                return result;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

public sealed class TransactionDecorator<TCommand>
    : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner;
    private readonly IApplicationDbContext _dbContext;

    public TransactionDecorator(ICommandHandler<TCommand> inner, IApplicationDbContext dbContext)
    {
        _inner = inner;
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await _inner.Handle(command, cancellationToken);

            if (!result.IsSuccess)
            {
                await transaction.RollbackAsync(cancellationToken);
                return result;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}