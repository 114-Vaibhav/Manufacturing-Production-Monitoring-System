using BusinessLayer.Exceptions;
using backend.Models;

public static class ProductionPlanValidator
{
    public static void ValidateProductionPlan(ProductionPlan plan)
    {
        if (plan == null)
        {
            throw new ValidationException("ProductionPlan object cannot be null.");
        }

        List<string> errors = new();

        if (string.IsNullOrWhiteSpace(plan.ProductName))
        {
            errors.Add("Product Name is required.");
        }
        else
        {
            plan.ProductName = plan.ProductName.Trim();

            if (plan.ProductName.Length < 2)
                errors.Add("Product Name must be at least 2 characters.");

            if (plan.ProductName.Length > 100)
                errors.Add("Product Name cannot exceed 100 characters.");
        }

        if (plan.TargetQuantity <= 0)
        {
            errors.Add("Target Quantity must be greater than 0.");
        }

        if (plan.StartDate == default)
        {
            errors.Add("Start Date is required.");
        }

        if (plan.EndDate == default)
        {
            errors.Add("End Date is required.");
        }

        if (plan.StartDate != default &&
            plan.EndDate != default &&
            plan.EndDate < plan.StartDate)
        {
            errors.Add("End Date cannot be earlier than Start Date.");
        }

        if (!Enum.IsDefined(typeof(ProductionPlanStatus), plan.Status))
        {
            errors.Add("Invalid Production Plan Status.");
        }

        if (plan.CreatedBy <= 0)
        {
            errors.Add("CreatedBy is required.");
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}