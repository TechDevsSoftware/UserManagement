﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TechDevs.Accounts.WebService.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/customer/account/register")]
    public class CustomerRegistrationController : Controller
    {
        private readonly IAuthUserService<Customer> _userService;

        public CustomerRegistrationController(IAuthUserService<Customer> userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] AuthUserRegistration registration, [FromHeader(Name = "TechDevs-ClientId")] string clientId)
        {
            try
            {
                if (registration == null) return new BadRequestObjectResult("Invalid registration");
                var result = await _userService.RegisterUser(registration, clientId);
                return new OkObjectResult(result);
            }
            catch (UserRegistrationException ex)
            {
                return new BadRequestObjectResult("Validaiton Errors: " + Environment.NewLine + ex.Message);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}