using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
  public Database Db { get; set; }
  
  private readonly IConfiguration _configuration;

  public UserController(Database db, IConfiguration configuration) {
    Db = db;
    _configuration = configuration;
  }

  protected string CreateToken(string username)
  {
    List<Claim> claims = new List<Claim>
    {
      new Claim(ClaimTypes.Name, username)
    };

    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
      _configuration.GetSection("AppSettings:Token").Value));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.Now.AddDays(1),
      signingCredentials: creds);

    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
  }

  [HttpGet("{username}/{password}")]
  public async Task<IActionResult> Get(string username, string password)
  {
    await Db.Connection.OpenAsync();
    var query = new User(Db);
    var user = await query.GetInfo(username);

    Token token = new Token();

    if (user.iduser == 0)
    {
      token.error = "The username you entered is not connected to an account";
      return Ok(token);
    }

    if (BCrypt.Net.BCrypt.Verify(password, user.password))
    {
      token.userID = user.iduser;
      token.token = CreateToken(username);
      token.error = "No error";

      return Ok(token);
    }

    token.error = "The password you entered is incorrect";
    return Ok(token);
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
}
