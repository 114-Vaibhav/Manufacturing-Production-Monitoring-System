using BusinessLayer.Exceptions;
using backend.Models;

public static class ProductValidator
{
    public static void ValidateProduct(Product product)
    {
        List<string> errors = new();

        if (product == null)
        {
            throw new ValidationException("Product object cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(product.ProductName))
        {
            errors.Add("Product Name is required.");
        }
        else
        {
            product.ProductName = product.ProductName.Trim();

            if (product.ProductName.Length < 2)
                errors.Add("Product Name must be at least 2 characters.");

            if (product.ProductName.Length > 100)
                errors.Add("Product Name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(product.ProductCode))
        {
            errors.Add("Product Code is required.");
        }
        else
        {
            product.ProductCode = product.ProductCode.Trim();

            if (product.ProductCode.Length > 50)
                errors.Add("Product Code cannot exceed 50 characters.");
        }

        if (!string.IsNullOrWhiteSpace(product.Description))
        {
            product.Description = product.Description.Trim();

            if (product.Description.Length > 500)
                errors.Add("Description cannot exceed 500 characters.");
        }

        if (product.UnitPrice < 0)
        {
            errors.Add("UnitPrice cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(product.Status))
        {
            errors.Add("Status is required.");
        }
        else
        {
            string[] validStatuses =
            {
                "Active",
                "Discontinued",
                "OutOfStock",
                "Inactive"
            };

            if (!validStatuses.Contains(
                    product.Status,
                    StringComparer.OrdinalIgnoreCase))
            {
                errors.Add(
                    $"Invalid Status. Allowed values: {string.Join(", ", validStatuses)}");
            }
        }

        // if (product.CreatedAt == default)
        // {
        //     errors.Add("CreatedAt is required.");
        // }
        // else if (product.CreatedAt > DateTime.UtcNow)
        // {
        //     errors.Add("CreatedAt cannot be a future date.");
        // }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
