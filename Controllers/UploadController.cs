using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Net.Http.Headers;

namespace movieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class UploadController : ControllerBase
{
  [HttpPost, DisableRequestSizeLimit]
  public IActionResult Upload()
  {
    try
    {
      var file = Request.Form.Files[0];
      
      if (file.Length > 0)
      {
        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fullPath = Path.Combine("wwwroot/images", fileName);
        var dbPath = Path.Combine("wwwroot/images", fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
          file.CopyTo(stream);
        }

        return Ok(new { dbPath });
      }
      else return BadRequest();
    }
    catch (Exception ex)
    {
      return StatusCode(500, $"Internal server error: {ex}");
    }
  }
}
