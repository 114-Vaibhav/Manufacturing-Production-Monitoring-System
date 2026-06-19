using BusinessLayer.Exceptions;
using backend.Models;

public static class ProductionOrderValidator
{
    public static void ValidateProductionOrder(ProductionOrder order)
    {
        if (order == null)
        {
            throw new ValidationException("ProductionOrder object cannot be null.");
        }

        List<string> errors = new();

        if (order.PlanId <= 0)
        {
            errors.Add("PlanId is required.");
        }

        if (order.MachineId <= 0)
        {
            errors.Add("MachineId is required.");
        }

        if (order.Quantity <= 0)
        {
            errors.Add("Quantity must be greater than 0.");
        }

        if (order.ProducedQuantity < 0)
        {
            errors.Add("ProducedQuantity cannot be negative.");
        }

        if (order.ProducedQuantity > order.Quantity)
        {
            errors.Add("ProducedQuantity cannot exceed Quantity.");
        }

        if (!Enum.IsDefined(typeof(ProductionOrderStatus), order.Status))
        {
            errors.Add("Invalid Production Order Status.");
        }

        if (errors.Any())
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Errors:");
            Console.WriteLine(string.Join("\n", errors));
            Console.ResetColor();
            throw new ValidationException(errors);
        }
    }
}