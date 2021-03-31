using System;
using System.Threading;
using System.Threading.Tasks;
using ExampleApi.Data;
using ExampleApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Person = ExampleApi.Data.Person;

namespace ExampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;

        public PersonController(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<Guid> Post([FromBody] NewPerson person, CancellationToken cancellationToken)
        {
            var newPerson = new Person(person.FirstName, person.LastName);
            await _dbContext.People.AddAsync(newPerson, cancellationToken);
            return newPerson.Id;
        }
    }
}