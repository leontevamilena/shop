using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopDbLibrary.Contexts;
using ShopDbLibrary.DTOs;
using ShopDbLibrary.Models;
using ShopDbLibrary.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopDbLibrary.Services
{
    public class AuthService(ShopDbContext context)
    {
        private readonly ShopDbContext _context = context;

        public async Task<string> GenerateTokenAsync(User user) // jwt токен
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            int minutes = 15;

            var role = await GetUserRoleByLoginAsync(user.Login);

            //данные для jwt
            var claims = new Claim[]
            {
                    new ("id", user.UserId.ToString()),
                    new ("login", user.Login),
                    new ("role", role.Name),
            };

            var token = new JwtSecurityToken( //создание токена
                signingCredentials: credentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //аутентификация пользователя
        public async Task<string?> AuthUserAsync(LoginReqest reqest)
        { 
            var login = reqest.Login;
            var password = reqest.Password;

            //если строка пустая, то вернёт null
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = await GetUserByLoginAsync(login);
            if (user is null)
                return null;

            //если всё прошло успешно, то пользователь авторизовывается и возвращается токен
            if (VerifyPassword(password, user.Password))
            {
                return await GenerateTokenAsync(user);
            }
            else
            {
                return null;
            }
        }

        //получение пользователя по логину
        private async Task<User?> GetUserByLoginAsync(string login) 
            => await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

        //проверка хэша пароля
        private bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

        //получение роли пользователя
        private async Task<Role?> GetUserRoleByLoginAsync(string login)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == login);

            // если пользователь существует, то вернётся его роль, а если пользователя нет, то null
            if (user is not null)
            {
                return user.Role;
            }
            return null; 
        }
    }
}
