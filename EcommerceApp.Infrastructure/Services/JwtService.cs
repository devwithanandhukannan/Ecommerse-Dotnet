using System.IdentityModel.Tokens.Jwt; // This namespace provides classes for creating and validating JSON Web Tokens (JWTs). The JwtSecurityTokenHandler class is used to create and validate JWTs.
using System.Security.Claims; // add user values/data to token
using System.Text; // This namespace provides classes for encoding and decoding text. The Encoding class is used to convert strings to byte arrays, which are needed for cryptographic operations.
using EcommerceApp.Application.Common.Interfaces; // This namespace contains the IJwtService interface, which defines the contract for generating JWTs.
using EcommerceApp.Domain.Entites; // This namespace contains the User class, which represents a user in the system. The GenerateToken method will use the properties of the User class to create a JWT.
using Microsoft.Extensions.Configuration; // This namespace provides classes for accessing configuration settings. The IConfiguration interface is used to read settings from configuration files, such as appsettings.json. In this case, it will be used to read the JWT secret key and other related settings.
using Microsoft.IdentityModel.Tokens; // This namespace provides classes for cryptographic operations related to JWTs. The SymmetricSecurityKey class is used to create a security key from the secret key, and the SigningCredentials class is used to specify the algorithm and key for signing the JWT.


namespace EcommerceApp.Infrastructure.Services;
public class JwtService : IJwtService{
    private readonly IConfiguration _configuration; // This field holds a reference to the configuration settings. It will be used to access the JWT secret key and other related settings.

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user){
        var secretKey = _configuration["Jwt:SecretKey"]; // This line retrieves the secret key from the configuration settings. The secret key is used to sign the JWT, ensuring its integrity and authenticity.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // This line creates a symmetric security key from the secret key. The Encoding.UTF8.GetBytes method converts the secret key string into a byte array, which is required for cryptographic operations.
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // This line creates signing credentials using the symmetric security key and the HMAC-SHA256 algorithm. The signing credentials are used to sign the JWT, ensuring that it cannot be tampered with.


        //data in that token (userid , email, role)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // This claim represents the subject of the JWT, which is the user's unique identifier. It is used to identify the user associated with the token.
            new Claim(JwtRegisteredClaimNames.Email, user.Email), // This claim represents the user's email address. It can be used for identification and communication purposes.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // This claim represents a unique identifier for the JWT itself. It is used to prevent replay attacks by ensuring that each token is unique.
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], // This line sets the issuer of the JWT, which is the entity that created and issued the token. It is retrieved from the configuration settings.
            audience: _configuration["Jwt:Audience"], // This line sets the audience of the JWT, which is the intended recipient of the token. It is also retrieved from the configuration settings.
            claims: claims, // This line specifies the claims to include in the JWT. The claims array contains information about the user and the token itself.
            expires: DateTime.UtcNow.AddHours(1), // This line sets the expiration time of the JWT to one hour from the current UTC time. After this time, the token will no longer be valid.
            signingCredentials: credentials // This line specifies the signing credentials to use for signing the JWT. The credentials ensure that the token cannot be tampered with and can be verified by the recipient.
        );

        return new JwtSecurityTokenHandler().WriteToken(token); // This line creates a new instance of the JwtSecurityTokenHandler class and uses it to write the JWT to a string format. The resulting string is the final JWT that can be sent to the client for authentication and authorization purposes.
    }
}
