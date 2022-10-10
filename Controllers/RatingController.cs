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
  public async Task<IActionResult> Get(string id)
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

  [HttpDelete()]
  public async Task<IActionResult> Delete(string id)
  {
    await Db.Connection.OpenAsync();
    var query = new Rating(Db);
    return new OkObjectResult(await query.DeleteRatings(id));
  }

  public Database Db { get; set; }
}
