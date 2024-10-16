using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dotnet8LineRichMenu.Models.MongoDB;
using Dotnet8LineRichMenu.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace Dotnet8LineRichMenu.Services.UserService
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly MongoDbSettings _mongoDbSettings;
        private readonly IConfiguration _configuration;

        public UserService(IOptions<MongoDbSettings> mongoDbSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _mongoDbSettings = mongoDbSettings.Value;

            var client = new MongoClient(_mongoDbSettings.ConnectionString);
            var database = client.GetDatabase(_mongoDbSettings.DatabaseName);
            _users = database.GetCollection<User>("Users");
        }

        // Register a new user
        public async Task<bool> RegisterUser(string email, string password)
        {
            // Check if user already exists
            var existingUser = await _users.Find(user => user.Email == email)
                .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return false;
            }

            // Hash the password using SHA256
            var passwordHash = ComputeSha256Hash(password);

            // Create a new user
            var newUser = new User
            {
                Username = email,
                Email = email,
                PasswordHash = passwordHash,
                IsVerified = false
            };

            await _users.InsertOneAsync(newUser);

            return true;
        }

        // Authenticate user and generate JWT token
        public async Task<string> Authenticate(string email, string password)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq("email", email),
                Builders<User>.Filter.Eq("isVerified", true)
            );

            var user = await _users.Find(filter).FirstOrDefaultAsync();

            if (user == null || user.PasswordHash != ComputeSha256Hash(password))
            {
                return null;
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMonths(4),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Helper function to compute SHA256 hash
        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256 hash object
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}