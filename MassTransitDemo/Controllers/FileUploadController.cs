using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using CsvHelper;
using MassTransitDemo.Messages;
using MassTransitDemo.Model;

namespace MassTransitDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly IBus _bus;

    public FileUploadController(IBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        List<Item> items;
        using (var reader = new StreamReader(file.OpenReadStream()))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            items = csv.GetRecords<Item>().ToList();
        }

        foreach (var item in items)
        {
            await _bus.Publish(new ProcessItemCommand
            {
                Item = item,
                At = DateTime.Now
            });
        }

        return Ok("File uploaded and processed.");
    }
}