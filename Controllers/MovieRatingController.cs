using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class MovieRatingController : ControllerBase
{
  public MovieRatingController(Database db) {
    Db = db;
  }

  [HttpPost()]
  public async Task<IActionResult> Post([FromBody] MovieRating body)
  {
    await Db.Connection.OpenAsync();
    body.Db = Db;
    string result = await body.SaveRatings();
    return new OkObjectResult(result);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(string id)
  {
    await Db.Connection.OpenAsync();
    var query = new MovieRating(Db);
    return new OkObjectResult(await query.DeleteRatings(id));
  }

  public Database Db { get; set; }
}
