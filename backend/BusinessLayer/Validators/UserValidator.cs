using System.Net.Mail;
using System.Text.RegularExpressions;
using BusinessLayer.Exceptions;
using backend.Models;
using backend.Models.DTOs;

public static class UserValidator
{
    public static void ValidateUser(RegisterUserRequest user)
    {
        List<string> errors = new();

        if (user == null)
        {
            throw new ValidationException("User object cannot be null.");
        }

        // ==========================
        // Full Name Validation
        // ==========================
        if (string.IsNullOrWhiteSpace(user.FullName))
        {
            errors.Add("Full Name is required.");
        }
        else
        {
            user.FullName = user.FullName.Trim();

            if (user.FullName.Length < 2)
                errors.Add("Full Name must be at least 2 characters.");

            if (user.FullName.Length > 100)
                errors.Add("Full Name cannot exceed 100 characters.");

            // Alphabets and spaces only
            if (!Regex.IsMatch(user.FullName, @"^[A-Za-z\s]+$"))
                errors.Add("Full Name can contain only letters and spaces.");

            // Prevent multiple spaces
            if (Regex.IsMatch(user.FullName, @"\s{2,}"))
                errors.Add("Full Name cannot contain multiple consecutive spaces.");
        }

        // ==========================
        // Email Validation
        // ==========================
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            errors.Add("Email is required.");
        }
        else
        {
            user.Email = user.Email.Trim();

            if (user.Email.Length > 255)
                errors.Add("Email cannot exceed 255 characters.");

            try
            {
                MailAddress mail = new(user.Email);

                if (mail.Address != user.Email)
                    errors.Add("Invalid email format.");
            }
            catch
            {
                errors.Add("Invalid email format.");
            }
        }

        // ==========================
        // Password Hash Validation
        // ==========================
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            errors.Add("Password hash is required.");
        }
        else
        {
            if (user.Password.Length < 8)
                errors.Add("Password must be at least 8 characters.");
        }

        // ==========================
        // Role Validation
        // ==========================
      

        // // ==========================
        // // Status Validation
        // // ==========================
        // if (string.IsNullOrWhiteSpace(user.Status))
        // {
        //     errors.Add("Status is required.");
        // }
        // else
        // {
        //     string[] validStatuses =
        //     {
        //         "Active",
        //         "Inactive",
        //         "Blocked"
        //     };

        //     if (!validStatuses.Contains(
        //             user.Status,
        //             StringComparer.OrdinalIgnoreCase))
        //     {
        //         errors.Add(
        //             $"Invalid Status. Allowed values: {string.Join(", ", validStatuses)}");
        //     }
        // }

        // ==========================
        // CreatedAt Validation
        // =========================
        // ==========================
        // Final Check
        // ==========================
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}