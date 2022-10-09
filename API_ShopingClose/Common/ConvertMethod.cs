using API_ShopingClose.Entities;
using API_ShopingClose.Model;

namespace API_ShopingClose.Common;

public class ConvertMethod
{
    public static Product convertProductModleToProduct(ProductModel productModel)
    {
        Product product = new Product();

        product.Image = productModel.image == null ? "" : productModel.image;
        product.ProductName = productModel.name;
        product.Slug = productModel.slug == null ? "" : productModel.slug;
        product.Description = productModel.description;
        product.Price = productModel.price;
        product.BrandID = productModel.brandId;
        product.Rate = productModel.rate;
        product.ProductID = productModel.productId;

        return product;
    }

    public static ProductModel convertProductToProductModel(Product product)
    {
        ProductModel productModel = new ProductModel();

        productModel.productId = product.ProductID;
        productModel.name = product.ProductName;
        productModel.slug = product.Slug;
        productModel.description = product.Description;
        productModel.price = product.Price;
        productModel.brandId = product.BrandID;
        productModel.rate = productModel.rate;

        return productModel;
    }

    public static ProductInCategory convertProductInCategoryModelToProductInCategory(
        ProductInCategoryModel productInCategoryModel)
    {
        ProductInCategory productInCategory = new ProductInCategory();

        productInCategory.productId = productInCategoryModel.productId;
        productInCategory.categoryId = productInCategoryModel.categoryId;

        return productInCategory;
    }
}