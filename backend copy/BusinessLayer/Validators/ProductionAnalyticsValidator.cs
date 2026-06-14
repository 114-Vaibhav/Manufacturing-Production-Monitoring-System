using BusinessLayer.Exceptions;
using backend.Models;

public static class ProductionAnalyticsValidator
{
    public static void ValidateProductionAnalytics(
        ProductionAnalytics productionAnalytics)
    {
        if (productionAnalytics == null)
        {
            throw new ValidationException(
                "ProductionAnalytics object cannot be null.");
        }

        List<string> errors = new();

        // MachineId validation
        if (productionAnalytics.MachineId <= 0)
        {
            errors.Add("MachineId must be greater than 0.");
        }

        // Efficiency validation (0% to 100%)
        if (productionAnalytics.Efficiency < 0 ||
            productionAnalytics.Efficiency > 100)
        {
            errors.Add(
                "Efficiency must be between 0 and 100.");
        }

        // Downtime validation
        if (productionAnalytics.Downtime < 0)
        {
            errors.Add(
                "Downtime cannot be negative.");
        }

        // DefectRate validation (0% to 100%)
        if (productionAnalytics.DefectRate < 0 ||
            productionAnalytics.DefectRate > 100)
        {
            errors.Add(
                "DefectRate must be between 0 and 100.");
        }

        // CalculatedDate validation
        if (productionAnalytics.CalculatedDate == default)
        {
            errors.Add(
                "CalculatedDate is required.");
        }
        else if (productionAnalytics.CalculatedDate >
                 DateTime.UtcNow)
        {
            errors.Add(
                "CalculatedDate cannot be a future date.");
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}