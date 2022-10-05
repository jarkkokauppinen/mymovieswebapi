using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class QuoteController : ControllerBase
{
  public QuoteController(Database db) {
    Db = db;
  }

  [HttpGet()]
  public async Task<IActionResult> Get()
  {
    await Db.Connection.OpenAsync();
    var query = new Quote(Db);
    return new OkObjectResult(await query.GetQuote());
  }

  public Database Db { get; set; }
}
