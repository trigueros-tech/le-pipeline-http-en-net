using System;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet("id")]
        public IActionResult Error(string id)
        {
            if (!int.TryParse(id, out _))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure("id", "id n'est pas un nombre valide")
                });
            }

            throw new Exception("Server exception");
        }
    }
}