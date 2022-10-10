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

  [HttpGet("{username}/{password}")]
  public async Task<IActionResult> Get(string username, string password)
  {
    await Db.Connection.OpenAsync();
    var query = new User(Db);
    var info = await query.GetInfo(username);

    if (info.iduser == 0)
    {
      return new OkObjectResult("The username you entered is not connected to an account");
    }

    if (BCrypt.Net.BCrypt.Verify(password, info.password))
    {
      return new OkObjectResult(info.iduser);
    }

    return new OkObjectResult("The password you entered is incorrect");
  }

  [HttpPost()]
  public async Task<IActionResult> NewUser([FromBody] User body)
  {
    await Db.Connection.OpenAsync();
    body.password = BCrypt.Net.BCrypt.HashPassword(body.password);
    body.Db = Db;
    string result = await body.CreateAccount();
    return new OkObjectResult(result);
  }

  public Database Db { get; set; }
}
