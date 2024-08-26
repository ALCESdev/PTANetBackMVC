using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BankController : ControllerBase
{
    #region VARIABLES

    private readonly BankService _bankService;

    #endregion

    #region CONSTRUCTOR

    public BankController(BankService bankService)
    {
        _bankService = bankService;
    }

    #endregion

    #region METHODS

    [HttpPost("fetch")]
    public async Task<IActionResult> FetchAndStoreBanks()
    {
        bool success = await _bankService.FetchAndStoreBanksAsync();

        if (success)
        {
            return Ok("Banks fetched and stored successfully.");
        }

        return StatusCode(500, "An error occurred while fetching or storing banks.");
    }

    #endregion
}