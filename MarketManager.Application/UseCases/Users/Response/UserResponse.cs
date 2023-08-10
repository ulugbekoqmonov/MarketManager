using MarketManager.Domain.Entities.Identity;
using Newtonsoft.Json;

namespace MarketManager.Application.UseCases.Users.Response;
public class UserResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Username { get; set; }
    public string? Picture { get; set; }
    public List<string>? RoleNames { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifyDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifyBy { get; set; }

    

}
