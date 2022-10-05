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
  public async Task<IActionResult> Get(int id)
  {
    await Db.Connection.OpenAsync();
    var query = new Director(Db);
    var director = await query.GetDirector(id);
    return new OkObjectResult(director);
  }

  [HttpPost()]
  public async Task<IActionResult> NewDirector([FromBody] Director body)
  {
    await Db.Connection.OpenAsync();
    body.Db = Db;
    string result = await body.SaveDirector();
    return new OkObjectResult(result);
  }

  public Database Db { get; set; }
}
