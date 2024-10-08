using calorieCounter_backend.Models;
using Google.Apis.Auth;

namespace calorieCounter_backend.Helpers;

public static class AuthHelper
{
    public static async Task<User> GetUserFromGoogleToken(string token)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(token);

        return new User(
            email: payload.Email,
            name: payload.Name
        );
    }
}