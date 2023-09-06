using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ci_cd_demo.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly SecretsList secrets;
    private readonly ILogger<CalculatorController> _logger;

    public CalculatorController(
        IOptions<SecretsList> secrets,
        ILogger<CalculatorController> logger)
    {
        this.secrets = secrets.Value;
        _logger = logger;
    }


    /// <summary>
    /// Perform x + y
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>Sum of x and y.</returns>
    [HttpGet("add/{x}/{y}")]
    public int Add(int x, int y)
    {
        _logger.LogInformation($"{x} plus {y} is {x + y}");
        return x + y;
    }

    /// <summary>
    /// Perform x - y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x subtract y</returns>
    [HttpGet("subtract/{x}/{y}")]
    public int Subtract(int x, int y)
    {
        _logger.LogInformation($"{x} subtract {y} is {x - y}");
        return x - y;
    }

    /// <summary>
    /// Perform x * y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x multiply y</returns>
    [HttpGet("multiply/{x}/{y}")]
    public int Multiply(int x, int y)
    {
        _logger.LogInformation($"{x} multiply {y} is {x * y}");
        return x * y;
    }

    /// <summary>
    /// Perform x / y.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>x divide y</returns>
    [HttpGet("divide/{x}/{y}")]
    public int Divide(int x, int y)
    {
        _logger.LogInformation($"{x} divide {y} is {x / y}");
        return x / y;
    }

    [HttpGet("test")]

    public string Test()
    {
        return $"The secret for this environment is {secrets.TestSecret}";
    }
}
