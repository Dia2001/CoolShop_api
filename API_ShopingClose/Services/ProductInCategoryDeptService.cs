using API_ShopingClose.Entities;
using Dapper;
using MySqlConnector;

namespace API_ShopingClose.Service;

public class ProductInCategoryDeptService
{
    private readonly MySqlConnection _conn;

    public ProductInCategoryDeptService(MySqlConnection conn)
    {
        _conn = conn;
    }

    public async Task<bool> InsertPrductInCategory(ProductInCategory productInCategory)
    {
        string sql = "INSERT INTO productincategory(ProductID, CategoryID)"
          + "VALUES (@ProductID, @CategoryID)";

        var parameters = new DynamicParameters();

        parameters.Add("@ProductID", productInCategory.productId);
        parameters.Add("@CategoryID", productInCategory.categoryId);

        return await _conn.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<bool> deleteProductInCategory(ProductInCategory productInCategory)
    {
        string sql = "DELETE FROM productincategory where ProductID = @ProductID and CategoryID = @CategoryID";

        var parameters = new DynamicParameters();

        parameters.Add("@ProductID", productInCategory.productId);
        parameters.Add("@CategoryID", productInCategory.categoryId);

        return await _conn.ExecuteAsync(sql, parameters) > 0;

    }

    public async Task<IEnumerable<ProductInCategory>> getProductInCategoryByProductID(Guid productID)
    {
        string sql = "SELECT * FROM productincategory where ProductID=@ProductID";
        var parameters = new DynamicParameters();
        parameters.Add("@ProductID", productID);

        return (await _conn.QueryAsync<ProductInCategory>(sql, parameters)).ToList();
    }
}
