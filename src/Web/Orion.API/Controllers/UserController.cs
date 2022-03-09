using Microsoft.AspNetCore.Mvc;
using Orion.API.Controllers.SeedWork;
using Orion.Application.UseCases.UserUseCases.GetUserById;
using System.Threading.Tasks;

namespace Orion.API.Controllers
{    
    public class UserController : OrionBaseController
    {

        [HttpGet("{id}")] //User/id
        public async Task<IActionResult> Get([FromRoute] int id)
        {
          var response = await  Mediator.Send(new GetUserByIdRequest { Id = id });
            return Ok(response);
        }

    }
}
