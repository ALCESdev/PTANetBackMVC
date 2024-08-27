using Microsoft.AspNetCore.Mvc;
using pt_alicunde_aae.Entities;

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

    // <summary>
    /// Fetches banks from example API and stores them in our database.
    /// </summary>
    /// <returns>HTTP 200 OK if successful; otherwise, HTTP 500 Internal Server Error.</returns>
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

    /// <summary>
    /// Gets all banks from the database.
    /// </summary>
    /// <returns>A list of all banks.</returns>
    [HttpGet("{All}")]
    public async Task<IActionResult> GetAllBanks() => Ok(await _bankService.GetAllBanksAsync());

    /// <summary>
    /// Gets a specific bank by id.
    /// </summary>
    /// <param name="id">The bank's id.</param>
    /// <returns>The requested bank if found; otherwise, HTTP 404 Not Found.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBankById(int id)
    {
        Bank? bank = await _bankService.GetBankByIdAsync(id);

        if (bank == null)
        {
            return NotFound("Bank not found.");
        }

        return Ok(bank);
    }

    /// <summary>
    /// Updates an existing bank.
    /// </summary>
    /// <param name="id">The bank's id to update.</param>
    /// <param name="updatedBank">The updated bank data.</param>
    /// <returns>HTTP 200 OK if successful; otherwise, HTTP 404 Not Found.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBank(int id, [FromBody] Bank updatedBank)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool success = await _bankService.UpdateBankAsync(id, updatedBank);

        if (!success)
        {
            return NotFound("Bank not found.");
        }

        return Ok("Bank updated successfully.");
    }

    /// <summary>
    /// Deletes a specific bank by its ID.
    /// </summary>
    /// <param name="id">The bank's id to delete.</param>
    /// <returns>HTTP 200 OK if successful; otherwise, HTTP 404 Not Found.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBank(int id)
    {
        bool success = await _bankService.DeleteBankAsync(id);

        if (!success)
        {
            return NotFound("Bank not found.");
        }

        return Ok("Bank deleted successfully.");
    }

    #endregion
}