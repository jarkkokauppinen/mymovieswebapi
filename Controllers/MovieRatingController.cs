using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class MovieRatingController : ControllerBase
{
  public Database Db { get; set; }
  
  public MovieRatingController(Database db) {
    Db = db;
  }

  [HttpGet("{idmovie}/{iduser}")]
  public async Task<bool> GetRow(string idmovie, int iduser)
  {
    await Db.Connection.OpenAsync();
    var query = new MovieRating(Db);
    var response = await query.GetRow(idmovie, iduser);
    
    if (response.idmovierating == 0)
    {
      return false;
    }

    return true;
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
}
