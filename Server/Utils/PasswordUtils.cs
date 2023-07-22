using ControleVendas.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ControleVendas.Server.Utils
{
    public static class PasswordUtils
    {
        public static void CreatePasswordHash(string senha, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
        }

        public static bool VerifyPasswordHash(Usuario usuario, string senha, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(usuario.Salt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            return computedHash.SequenceEqual(hash);
        }

        public static string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new("Id", usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.Nome),
            };

            var superSecretKey = GetSuperSecretKey();

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(superSecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GetSuperSecretKey()
            => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is var env
                && env == "Development" || env is null
                ? string.Concat(Enumerable.Repeat("nyan", 128))
                : Environment.GetEnvironmentVariable("SUPER_SECRET_TOKEN");
    }
}
