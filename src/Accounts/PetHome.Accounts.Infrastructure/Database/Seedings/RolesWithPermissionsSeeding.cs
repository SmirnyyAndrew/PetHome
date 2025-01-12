using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHome.Accounts.Domain.Accounts;
using PetHome.Accounts.Domain.Aggregates;
using PetHome.Core.ValueObjects.RolePermission;
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PetHome.Accounts.Infrastructure.Database.Seedings;
public static class RolesWithPermissionsSeeding
{
    public static string JSON_FOLDER_PATH = "..\\Accounts\\PetHome.Accounts.Infrastructure\\Auth\\Permissions\\JsonConfigurations\\";
    public static string ROLES_JSON_FILE_NAME = "Roles.json";
    public static string PERMISSIONS_JSON_FILE_NAME = "Permissions.json";
    public static string PERMISSION_GROUP_NAME = "Permissions";
    public static string ROLES_GROUP_NAME = "Roles";
    public static string ROLES_ITEMS_NAME = "Items";

    private static List<Role> _roles = new List<Role>();
    private static List<Permission> _permissions = new List<Permission>();

    public static IServiceCollection SeedRolesWithPermissions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AuthorizationDbContext dbContext = new AuthorizationDbContext();

        if (dbContext.Roles.Count() == 0)
        {
            SeedRoles(dbContext);
            SeedPermissions(dbContext);

            dbContext.Roles.AddRange(_roles);
            dbContext.Permissions.AddRange(_permissions);

            var rolesPermissions = RolePermission.Create(_roles);

            dbContext.RolesPermissions.AddRange(rolesPermissions);
            dbContext.SaveChanges();
        }

        return services;
    }

    private static void SeedRoles(AuthorizationDbContext dbContext)
    {
        string roleJsonPath = JSON_FOLDER_PATH + ROLES_JSON_FILE_NAME;
        string jsonRolesString = File.ReadAllText(roleJsonPath);
        var jsonRolesObject = JsonNode.Parse(jsonRolesString);

        if (jsonRolesObject is not null)
        {
            var rolesStrings = jsonRolesObject?[ROLES_GROUP_NAME]?[ROLES_ITEMS_NAME]?.AsArray()?.Deserialize<string[]>();
            _roles = rolesStrings?.Select(r => Role.Create(r).Value).ToList();
        }
    }

    private static async void SeedPermissions(AuthorizationDbContext dbContext)
    {
        string permissionJsonPath = JSON_FOLDER_PATH + PERMISSIONS_JSON_FILE_NAME;
        string jsonPermissionString = File.ReadAllText(permissionJsonPath);
        var jsonPermissionObject = JsonNode.Parse(jsonPermissionString);
         
        var permissionStrings = jsonPermissionObject?[PERMISSION_GROUP_NAME]?[AdminAccount.ROLE]?
                    .AsArray()?.Deserialize<string[]>().ToList();
        _permissions = permissionStrings?
                .Distinct()
                .Select(r => Permission.Create(r).Value).ToList();

        if (_permissions?.Count == 0)
            return;

        foreach (var role in _roles)
        {
            var rolePermissionStrings = jsonPermissionObject?[PERMISSION_GROUP_NAME]?[role.Name]?
                .AsArray()?.Deserialize<string[]>().ToList();

            List<Permission>? rolePermissions = _permissions!.Where(x => rolePermissionStrings.Contains(x.Code.Value)).ToList();

            if (rolePermissions is not null && rolePermissions.Count > 0)
            {
                role.SetPermissions(rolePermissions); 
            }
        }

    }
}

