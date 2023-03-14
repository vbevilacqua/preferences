namespace Api.Tests.ApplicationFactory
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

    public static class FakeJwtManager
    {
        public static string Issuer { get; } = "https://dev-vhzksaal666an7qf.us.auth0.com/";
        public static string Audience { get; } = Guid.NewGuid().ToString();
        public static SecurityKey SecurityKey { get; }
        public static SigningCredentials SigningCredentials { get; }

        private static readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private static readonly RandomNumberGenerator generator = RandomNumberGenerator.Create();
        private static readonly byte[] key = new byte[32];

        static FakeJwtManager()
        {
            generator.GetBytes(key);
            SecurityKey = new SymmetricSecurityKey(key) { KeyId = Guid.NewGuid().ToString() };
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public static string GenerateJwtToken()
        {
            return tokenHandler.WriteToken(new JwtSecurityToken(Issuer, Audience, new Claim[] { new Claim("scope", "write:solution read:solution write:user read:user write:preference read:preference") }, null, DateTime.UtcNow.AddMinutes(10), SigningCredentials));
        }
    }
}
