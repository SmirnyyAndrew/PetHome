using System.Net;
using System.Net.Http.Json;

namespace PetHome.Accounts.Contracts.HttpCommunication;
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
}
