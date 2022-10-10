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
  public async Task<IActionResult> Get(string id)
  {
    await Db.Connection.OpenAsync();
    var query = new Actor(Db);
    return new OkObjectResult(await query.GetActors(id));
  }

  [HttpGet("{firstname}/{lastname}")]
  public async Task<IActionResult> GetOne(string firstname, string lastname)
  {
    await Db.Connection.OpenAsync();
    var query = new Actor(Db);
    return new OkObjectResult(await query.GetActorByName(firstname, lastname));
  }

  [HttpPost()]
  public async Task<IActionResult> Post([FromBody] Actor body)
  {
    await Db.Connection.OpenAsync();
    var query = new Actor(Db);
    var actor = await query.GetActorByName(body.firstname, body.lastname);

    if (actor.idactor is null)
    {
    body.Db = Db;
    string result = await body.SaveActor(body.idactor);
    return new OkObjectResult(result);
    }

    return new OkObjectResult(actor.idactor);
  }

  public Database Db { get; set; }
}
