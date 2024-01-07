using Dapper;
using Discount.API.Entities;
using Discount.API.Extensions;
using Microsoft.Data.SqlClient;
using VaultSharp;

namespace Discount.API.Repositories
{
    public class MsDiscountRepository : IDiscountRepository
    {
        private readonly VaultExtensions Client;
        private readonly string cString;

        public MsDiscountRepository(VaultExtensions vaultClient)
        {
            Client = vaultClient;
            cString = Client.GetConfig("dicountDb").ToString();
        }

        public async Task<List<Coupon>> GetAllDiscount()
        {

            var s = Client.GetDatabaseCredentials();
            using var connection = new SqlConnection(s);
            var result = await connection.QueryAsync<Coupon>("SELECT * FROM Coupon");

            return result.ToList();
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new SqlConnection(cString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
                return new Coupon
                { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new SqlConnection(cString);
            var affected =
                await connection.ExecuteAsync
                    ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new SqlConnection(cString);

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new SqlConnection(cString);

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }
    }
}