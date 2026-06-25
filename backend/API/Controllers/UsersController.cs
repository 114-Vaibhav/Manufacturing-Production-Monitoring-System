using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.DataAccessLayer;
using backend.Models;
using BusinessLayer.Interfaces;
using backend.Models.DTOs;
using BusinessLayer.Exceptions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserServices _userServices;

        public UsersController( IUserServices userServices)
        {
            _userServices = userServices;
        }

    
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userServices.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/user-register")]
        public async Task<ActionResult<RegisterUserResponse>> PostUser(RegisterUserRequest user)
        {
            try
            {   
                var createdUser = await _userServices.RegisterUser(user);
                return Ok(createdUser);
            }
            catch (ValidationException ex)
            {

                return BadRequest(new

                {

                    message = ex.Message,

                    errors = ex.Errors

                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("/user-login")]
        public async Task<ActionResult<LoginResponse>> LoginUser(LoginRequest user)
        {
            Console.WriteLine($"User {user.UserName} logging in.");
                Console.WriteLine($"User {user.Role} logging in.");
            try
            {   
                Console.WriteLine($"User {user.UserName} logging in.");
                Console.WriteLine($"User {user.Role} logging in.");

                var loggedInUser = await _userServices.LoginUser(user);
                
                return Ok(loggedInUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         }     

        [HttpPost("/update-password")]
        public async Task<ActionResult<LoginResponse>> UpdatePassword(UpdatePasswordRequest request)
        {
            try
            {   
                var loggedInUser = await _userServices.UpdatePassword(request);
                return Ok(loggedInUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         }

    }
}
