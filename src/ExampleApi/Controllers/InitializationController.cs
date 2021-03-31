using System;
using System.Threading;
using System.Threading.Tasks;
using ExampleApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InitializationController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;

        public InitializationController(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task CreateFamily(CancellationToken cancellationToken)
        {
            await _dbContext.People.AddAsync(new Person("Rick", "Sanchez"), cancellationToken);
            await _dbContext.People.AddAsync(new Person("Morty", "Smith"), cancellationToken);
            await _dbContext.People.AddAsync(new Person("Summer", "Smith"), cancellationToken);
            await _dbContext.People.AddAsync(new Person("Beth", "Smith"), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _dbContext.People.AddAsync(new Person("Jerry", "Smith"), cancellationToken);
            throw new Exception("On est dans la saison 3");
        }
    }
}