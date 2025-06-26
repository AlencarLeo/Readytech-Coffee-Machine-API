using Microsoft.AspNetCore.Mvc;
using ReadyTech.src.Application.Interfaces;

namespace ReadyTech.src.Api.Controllers;

[Route("/")]
[ApiController]
public class CoffeeController : ControllerBase
{
    private readonly ICoffeeService _coffeeService;

    public CoffeeController(ICoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
    }

    [HttpGet]
    [Route("brew-coffee")]
    public async Task<IActionResult> BrewCoffee()
    {
        try
        {
            var result = await _coffeeService.BrewCoffee();
            return StatusCode(result.StatusCode, result.Response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}