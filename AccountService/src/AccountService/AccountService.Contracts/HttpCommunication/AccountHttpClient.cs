using AccountService.Contracts.HttpCommunication.Dto;
using PetHome.Core.Response.RefreshToken;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AccountService.Contracts.HttpCommunication;
public class AccountHttpClient(HttpClient httpClient)
{
    public async Task<string>? GetUserEmailByUserId(Guid userId, CancellationToken ct)
    {
        var response = await httpClient.GetAsync($"user-by-email/{userId}", ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return string.Empty;

        string email = await response.Content.ReadAsStringAsync(ct);
        return email;
    }

    public async Task<Guid>? GetRoleIdByName(string name, CancellationToken ct)
    {
        var response = await httpClient.GetAsync($"role-id-by-name/{name}", ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return Guid.Empty;

        string getRoleIdResult = await response.Content.ReadAsStringAsync(ct);
        bool result = Guid.TryParse(getRoleIdResult, out Guid roleId);
        if (result is false)
            return Guid.Empty;

        return roleId;
    }

    public async Task<string>? GenerateAccessToken(Guid userId, CancellationToken ct)
    {
        var response = await httpClient.PostAsJsonAsync($"access-token", userId, ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return string.Empty;

        string accessToken = await response.Content.ReadFromJsonAsync<string>(ct);
        return accessToken;
    }

    //public async Task<RefreshSession>? GenerateRefreshToken(Guid userId, string accessToken, CancellationToken ct)
    //{
    //    var response = await httpClient.PostAsJsonAsync($"refresh-token", new { userId, accessToken }, ct);

    //    if (response.StatusCode != HttpStatusCode.OK)
    //        return null;

    //    RefreshSession refreshSession = await response.Content.ReadFromJsonAsync<RefreshSession>(ct);
    //    return refreshSession;
    //}

    public async Task<IReadOnlyList<string>>? GetUserPermissionsCodes(Guid userId, CancellationToken ct)
    {
        var response = await httpClient.GetAsync($"user-permissions-codes/{userId.ToString()}", ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return [];

        IReadOnlyList<string> userPermissions = await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>(ct); 
        return userPermissions;
    }

    public async Task<string>? GetUserRoleName(Guid userId, CancellationToken ct)
    {
        var response = await httpClient.GetAsync($"user-role-name/{userId}", ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return string.Empty;

        string email = await response.Content.ReadAsStringAsync(ct);
        return email;
    }

    public async Task<UserDto>? GetUserMainInformation(Guid userId, CancellationToken ct)
    {
        var response = await httpClient.GetAsync($"user-main-information/{userId}", ct);

        if (response.StatusCode != HttpStatusCode.OK)
            return null;

        UserDto userDto = await response.Content.ReadFromJsonAsync<UserDto>(ct);
        return userDto;
    }
}
