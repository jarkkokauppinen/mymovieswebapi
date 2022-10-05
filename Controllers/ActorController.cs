using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class ActorController : ControllerBase
{
  public ActorController(Database db) {
    Db = db;
  }

  [HttpGet()]
  public async Task<IActionResult> Get(int id)
  {
    await Db.Connection.OpenAsync();
    var query = new Actor(Db);
    return new OkObjectResult(await query.GetActors(id));
  }

  [HttpPost()]
  public async Task<IActionResult> Post([FromBody] Actor body)
  {
    await Db.Connection.OpenAsync();
    body.Db = Db;
    string result = await body.SaveActor();
    return new OkObjectResult(result);
  }

  public Database Db { get; set; }
}
