using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Identity;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.CustomErrors;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Authentication.Commands.Register;

public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserManagerProvider _userManager;

    public RegisterCommandHandler(IApplicationDbContext dbContext, IUserManagerProvider userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user is not null)
        {
            return Result<Guid>.Failure(UserErrors.UserAlreadyExists(command.Email));
        }

        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = command.Email,
            UserName = command.Email,
        };

        await _userManager.CreateUser(newUser, command.Password);
        return Result<Guid>.Success(newUser.Id);
    }
}
