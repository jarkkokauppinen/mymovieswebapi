using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class GenreController : ControllerBase
{
  public GenreController(Database db) {
    Db = db;
  }

  [HttpGet()]
  public async Task<IActionResult> Get(int id)
  {
    await Db.Connection.OpenAsync();
    var query = new Genre(Db);
    return new OkObjectResult(await query.GetGenre(id));
  }

  public Database Db { get; set; }
}
