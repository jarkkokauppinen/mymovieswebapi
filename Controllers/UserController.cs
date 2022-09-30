using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  public UserController(Database db) {
    Db = db;
  }

  [HttpPost()]
  public async Task<IActionResult> NewUser([FromBody] User body)
  {
    await Db.Connection.OpenAsync();
    body.password = BCrypt.Net.BCrypt.HashPassword(body.password);
    body.Db = Db;
    string result = await body.CreateAccount();
    if (result == "not ok") return new ConflictObjectResult(result);
    return new OkObjectResult(result);
  }

  public Database Db { get; set; }
}
