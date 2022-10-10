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
  public async Task<IActionResult> GetOne(string id)
  {
    await Db.Connection.OpenAsync();
    var query = new Movie(Db);
    var result = await query.GetById(id);
    return new OkObjectResult(result);
  }

  [HttpPost()]
  public async Task<IActionResult> NewMovie([FromBody] Movie body)
  {
    await Db.Connection.OpenAsync();
    body.Db = Db;
    string result = await body.PostMovie();
    return new OkObjectResult(result);
  }

  [HttpPut()]
  public async Task<IActionResult> Update([FromBody] Movie body)
  {
    await Db.Connection.OpenAsync();
    
    var movie = new Movie(Db)
    {
      idmovie = body.idmovie,
      title = body.title,
      year = body.year,
      description = body.description,
      image = body.image,
      iddirector = body.iddirector,
      idgenre = body.idgenre,
      iduser = body.iduser
    };

    await movie.UpdateMovie();
    return new OkObjectResult(movie);
  }

  [HttpDelete()]
  public async Task<IActionResult> Delete(string id)
  {
    await Db.Connection.OpenAsync();
    var query = new Movie(Db);
    return new OkObjectResult(await query.DeleteMovie(id));
  }

   public Database Db { get; set; }
}
