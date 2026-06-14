using BusinessLayer.Exceptions;
using backend.Models;

public static class DefectValidator
{
    public static void ValidateDefect(Defect defect)
    {
        List<string> errors = new();

        if (defect == null)
        {
            throw new ValidationException("Defect object cannot be null.");
        }

        if (defect.OrderId <= 0)
        {
            errors.Add("Order is required.");
        }

        if (defect.MachineId <= 0)
        {
            errors.Add("Machine is required.");
        }

        // if (defect.ReportedBy <= 0)
        // {
        //     errors.Add("ReportedBy is required.");
        // }

        if (string.IsNullOrWhiteSpace(defect.Description))
        {
            errors.Add("Description is required.");
        }
        else
        {
            defect.Description = defect.Description.Trim();

            if (defect.Description.Length > 500)
                errors.Add("Description cannot exceed 500 characters.");
        }

        // if (defect.CreatedAt == default)
        // {
        //     errors.Add("CreatedAt is required.");
        // }
        // else if (defect.CreatedAt > DateTime.UtcNow)
        // {
        //     errors.Add("CreatedAt cannot be a future date.");
        // }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
