using bmAPI.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace bmAPI.Extensions
{
    public static class DataEndpointExtensions
    {
        private static readonly Regex PascalToSnakeRegex = new Regex("([a-z])([A-Z])", RegexOptions.Compiled);
        public static string ToStoredProcedureName(this DataEndpoint endpoint)
        {         
            var snakeCase = PascalToSnakeRegex.Replace(endpoint.ToString(), "$1_$2").ToLower();

            return $"ReturnData_{snakeCase}";
        }
    }
}
