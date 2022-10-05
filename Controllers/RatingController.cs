using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class RatingController : ControllerBase
{
  public RatingController(Database db) {
    Db = db;
  }

  [HttpGet()]
  public async Task<IActionResult> Get(int id)
  {
    await Db.Connection.OpenAsync();
    var query = new Rating(Db);
    return new OkObjectResult(await query.GetRatings(id));
  }

  [HttpPost()]
  public async Task<IActionResult> Post([FromBody] Rating body)
  {
    await Db.Connection.OpenAsync();
    body.Db = Db;
    string result = await body.SaveRatings();
    return new OkObjectResult(result);
  }

  public Database Db { get; set; }
}
