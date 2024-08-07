
using Microsoft.AspNetCore.Mvc;

namespace Snacks.Api.Controllers;

[ApiController]
[Route("/[controller]")]
public class ImagesController : Controller
{
    [HttpGet("{imageName}")]
    public ActionResult GetImage(string imageName)
    {
        var imagePath = "./Images/" + imageName;

        if (System.IO.File.Exists(imagePath))
        {
            var imageType = "image/" + imageName.Split('.')[1];
            try
            {
                var imageData = System.IO.File.ReadAllBytes(imagePath);
                return File(imageData, imageType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading image: {ex.Message}");
                return StatusCode(500); // Internal server error for file read failure
            }
        }
        else
        {
            return NotFound(); 
        }
    }
}
