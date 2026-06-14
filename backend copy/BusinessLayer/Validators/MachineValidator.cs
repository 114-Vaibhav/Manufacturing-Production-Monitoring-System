using BusinessLayer.Exceptions;
using backend.Models;

public static class MachineValidator
{
    public static void ValidateMachine(Machine machine)
    {
        List<string> errors = new();

        if (machine == null)
        {
            throw new ValidationException("Machine object cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(machine.MachineName))
        {
            errors.Add("Machine Name is required.");
        }
        else
        {
            machine.MachineName = machine.MachineName.Trim();

            if (machine.MachineName.Length < 2)
                errors.Add("Machine Name must be at least 2 characters.");

            if (machine.MachineName.Length > 100)
                errors.Add("Machine Name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(machine.MachineCode))
        {
            errors.Add("Machine Code is required.");
        }
        else
        {
            machine.MachineCode = machine.MachineCode.Trim();

            if (machine.MachineCode.Length > 50)
                errors.Add("Machine Code cannot exceed 50 characters.");
        }

        if (machine.LocationId<=0)
        {
            errors.Add("LocationId is required.");
        }
        else
        {
            if (machine.LocationId > 1e5)
                errors.Add("LocationId cannot exceed 100000.");
        }

        if (machine.Status==MachineStatus.Running || machine.Status==MachineStatus.Maintenance
         || machine.Status==MachineStatus.OutOfService || machine.Status==MachineStatus.Idle)
        {
            // Valid status
        }
        else
        {
            errors.Add("Status is required.");
        }

        if (machine.LastMaintenanceDate.HasValue &&
            machine.LastMaintenanceDate.Value > DateTime.UtcNow)
        {
            errors.Add("LastMaintenanceDate cannot be a future date.");
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
