﻿using EmployeeManagementAPI.Controllers.ActionFilters;
using EmployeeManagementAPI.Services.Contracts;
using EmployeeManagementAPI.SharedResources.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CodeMaze.Api.Controllers.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;
        public AuthenticationController(IServiceManager service) => _service = service;

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        //[HttpPost("login")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        //{
        //    if (!await _service.AuthenticationService.ValidateUser(user))
        //        return Unauthorized();

        //    return Ok(new { Token = await _service.AuthenticationService.CreateToken() });
        //}

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _service.AuthenticationService.ValidateUser(user))
                return Unauthorized();

            var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

            return Ok(tokenDto);
        }
    }
}
