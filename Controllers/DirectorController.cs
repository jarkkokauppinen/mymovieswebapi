using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class DirectorController : ControllerBase
{
  public DirectorController(Database db) {
    Db = db;
  }

  [HttpGet()]
  public async Task<IActionResult> Get(string id)
  {
    await Db.Connection.OpenAsync();
    var query = new Director(Db);
    var director = await query.GetDirector(id);
    return new OkObjectResult(director);
  }

  [HttpGet("{firstname}/{lastname}")]
  public async Task<IActionResult> GetOne(string firstname, string lastname)
  {
    await Db.Connection.OpenAsync();
    var query = new Director(Db);
    return new OkObjectResult(await query.GetOneDirector(firstname, lastname));
  }

  [HttpPost()]
  public async Task<IActionResult> NewDirector([FromBody] Director body)
  {
    await Db.Connection.OpenAsync();
    var query = new Director(Db);
    var director = await query.GetOneDirector(body.firstname, body.lastname);
    
    if (director.iddirector is null)
    {
    body.Db = Db;
    string result = await body.SaveDirector(body.iddirector);
    return new OkObjectResult(result);
    }

    return new OkObjectResult(director.iddirector);
  }

  public Database Db { get; set; }
}
