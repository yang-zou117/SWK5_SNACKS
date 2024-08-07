using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    // Secret key used for encoding and decoding the JWT token
    private const string SecretKey = "SWK_SnacksSecretKey123456789101112131415161718192021222324252627282930"; 
    private static readonly SymmetricSecurityKey SecurityKey = 
        new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

    public static string EncodeToken(Dictionary<string, string> parameters)
    {
        var claims = new List<Claim>();

        // Convert provided parameters into claims to be added to the token
        foreach (var parameter in parameters)
        {
            claims.Add(new Claim(parameter.Key, parameter.Value));
        }

        // Create a token descriptor specifying the claims and signing credentials
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        // create and write jwt token
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }

    public static Dictionary<string, string> DecodeToken(string token)
    {
        // Read and parse the provided token as a JwtSecurityToken
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        // Check if the token is invalid
        if (securityToken == null)
        {
            throw new ArgumentException("Invalid token");
        }

        var claims = new Dictionary<string, string>();

        // Extract claims from the security token and store them in a dictionary
        foreach (var claim in securityToken.Claims)
        {
            claims.Add(claim.Type, claim.Value);
        }

        return claims;
    }
}
