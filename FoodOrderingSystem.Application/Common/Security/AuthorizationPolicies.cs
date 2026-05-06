namespace FoodOrderingSystem.Application.Common.Security;

public static class AuthorizationPolicies
{
    public static string AdminPriority => "AdminPriority";
    public static string CustomerPriority => "CustomerPriority";
}
