using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Exceptions;
using backend.Models;

public static class MachineReadingValidator
{
    public static void ValidateMachineReading(MachineReading machineReading)
    {
        if (machineReading == null)
        {
            throw new ValidationException("MachineReading object cannot be null.");
        }

        List<string> errors = new();

        // MachineId validation
        if (machineReading.MachineId <= 0)
        {
            errors.Add("MachineId must be greater than 0.");
        }

        // Temperature validation
        if (machineReading.Temperature < -100 || machineReading.Temperature > 1000)
        {
            errors.Add("Temperature must be between -100 and 1000.");
        }

        // Vibration validation
        if (machineReading.Vibration < 0)
        {
            errors.Add("Vibration cannot be negative.");
        }

        // PowerConsumption validation
        if (machineReading.PowerConsumption < 0)
        {
            errors.Add("PowerConsumption cannot be negative.");
        }

        // Timestamp validation
        // if (machineReading.Timestamp == default)
        // {
        //     errors.Add("Timestamp is required.");
        // }
        // else if (machineReading.Timestamp > DateTime.UtcNow)
        // {
        //     errors.Add("Timestamp cannot be in the future.");
        // }

        // Throw exception if any validation errors exist
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}