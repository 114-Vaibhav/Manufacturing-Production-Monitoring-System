using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.DTOs
{
    public class RegisterUserResponse
    {
        public int UserId { get; set; }
        public UserRole Role { get; set; }

    }
}