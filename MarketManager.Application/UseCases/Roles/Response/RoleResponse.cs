namespace MarketManager.Application.UseCases.Roles.Response;
public class RoleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }    
    public List<string>? PermissionNames { get; set; }
}
