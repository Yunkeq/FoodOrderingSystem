using System.Data;

namespace FoodOrderingSystem.Application.Abstractions.Db;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
