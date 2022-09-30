using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
  public MovieController(Database db) {
    Db = db;
  }

  [HttpGet("search/{title}")]
  public async Task<IActionResult> Get(string title)
  {
    await Db.Connection.OpenAsync();
    var query = new Movie(Db);
    var result = await query.GetSearched(title);
    return new OkObjectResult(result);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetOne(int id)
  {
    await Db.Connection.OpenAsync();
    var query = new Movie(Db);
    var result = await query.GetById(id);
    return new OkObjectResult(result);
  }

  public Database Db { get; set; }
}
