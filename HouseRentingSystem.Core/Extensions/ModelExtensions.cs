using HouseRentingSystem.Core.Contracts;
using System.Text;
using System.Text.RegularExpressions;

namespace HouseRentingSystem.Core.Extensions
{
    public static class ModelExtensions
    {
        public static string GetInformation(this IHouseModel house)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(house.Title.Replace(' ', '-'));
            sb.Append('-');
            sb.Append(GetAddress(house.Address));

            return sb.ToString().Trim();
        }

        private static string GetAddress(string address)
        {
            string result = string
                .Join('-', address.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(3));
            Regex.Replace(result, @"[^a-zA-Z0-9\-]", string.Empty);

            return result;
        }
    }
}
