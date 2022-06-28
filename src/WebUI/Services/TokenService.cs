namespace Carpool.WebUI.Services;

public class TokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(ApplicationUser user)
    {
        var claims = new List<Claim>{
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTTokenKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string DecodeToken(string token)
    {
        var decodeTokenBytes = WebEncoders.Base64UrlDecode(token);
        return Encoding.UTF8.GetString(decodeTokenBytes);
    }

    public string EncodeToken(string token)
    {
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
    }
}
