using BookShelfApp.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace BookShelfApp.Helpers
{
    public static class UtilitySecurity
    {

        public static bool IsGuid(string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }

        public static bool IsGuid(Guid value)
        {
            Guid x;
            return Guid.TryParse(value.ToString(), out x);
        }


        public static string ReplaceParameters(string inputString)
        {
            var inputArr = inputString.Split(',');
            var str = "";
            var count = 0;
            foreach (var item in inputArr)
            {
                str = str + item + "={" + count + "}&";
                count++;
            }

            var result = str.TrimEnd('&');

            return result;
        }


        public static string GenerateToken(User user)
        {
            //Generating Token



            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Super Secret Key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                       new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                       new Claim(ClaimTypes.Name, user.EmailAddress)
                }),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public static string GenerateApiKey()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            return Convert.ToBase64String(key);
        }

        public static string GenerateRandomNumber(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            var udu = Convert.ToBase64String(hash);
            return GetStringFromHash(hash);
        }

        public static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString().ToLower();
        }

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            var udu = Convert.ToBase64String(hash);
            return GetStringFromHash(hash);
        }

        public static string GenerateSHA512(string inputString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = null;
            string udu = "";
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(bytes);
                udu = Convert.ToBase64String(hash);
            }
            return udu;
        }
    }

    public static class LogHelper
    {
        public static string ToJson(this object value)
        {
            if (value == null) return "{ }";
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, settings);
        }
    }

    public static class Retry
    {
        public static void Do(Action action, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }
}
