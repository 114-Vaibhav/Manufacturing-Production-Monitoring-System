using BusinessLayer.Exceptions;
using backend.Models;

public static class ProductionRecordValidator
{
    public static void ValidateProductionRecord(ProductionRecord record)
    {
        if (record == null)
        {
            throw new ValidationException("ProductionRecord object cannot be null.");
        }

        List<string> errors = new();

        if (record.ProductionPlanId <= 0)
        {
            errors.Add("ProductionPlanId is required.");
        }

        if (record.ProducedQuantity <= 0)
        {
            errors.Add("ProducedQuantity must be greater than 0.");
        }

        if (record.ProductionDate == default)
        {
            errors.Add("ProductionDate is required.");
        }
        else if (record.ProductionDate > DateTime.UtcNow)
        {
            errors.Add("ProductionDate cannot be in the future.");
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}