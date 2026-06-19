using BusinessLayer.Exceptions;
using backend.Models;

public static class MaintenanceLogValidator
{
    public static void ValidateMaintenanceLog(MaintenanceLog maintenanceLog)
    {
        if (maintenanceLog == null)
        {
            throw new ValidationException("MaintenanceLog object cannot be null.");
        }

        List<string> errors = new();

        // MachineId validation
        if (maintenanceLog.MachineId <= 0)
        {
            errors.Add("MachineId must be greater than 0.");
        }

        // // EngineerId validation
        // if (maintenanceLog.EngineerId <= 0)
        // {
        //     errors.Add("EngineerId must be greater than 0.");
        // }

        // IssueDescription validation
        if (string.IsNullOrWhiteSpace(maintenanceLog.IssueDescription))
        {
            errors.Add("Issue Description is required.");
        }
        else
        {
            maintenanceLog.IssueDescription =
                maintenanceLog.IssueDescription.Trim();

            if (maintenanceLog.IssueDescription.Length < 5)
            {
                errors.Add("Issue Description must be at least 5 characters.");
            }

            if (maintenanceLog.IssueDescription.Length > 1000)
            {
                errors.Add("Issue Description cannot exceed 1000 characters.");
            }
        }

        // Resolution validation
        if (string.IsNullOrWhiteSpace(maintenanceLog.Resolution))
        {
            errors.Add("Resolution is required.");
        }
        else
        {
            maintenanceLog.Resolution =
                maintenanceLog.Resolution.Trim();

            if (maintenanceLog.Resolution.Length < 5)
            {
                errors.Add("Resolution must be at least 5 characters.");
            }

            if (maintenanceLog.Resolution.Length > 1000)
            {
                errors.Add("Resolution cannot exceed 1000 characters.");
            }
        }

        // // MaintenanceDate validation
        // if (maintenanceLog.MaintenanceDate == default)
        // {
        //     errors.Add("Maintenance Date is required.");
        // }
        // else if (maintenanceLog.MaintenanceDate > DateTime.UtcNow)
        // {
        //     errors.Add("Maintenance Date cannot be in the future.");
        // }

        // Throw validation exception if any errors exist
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}