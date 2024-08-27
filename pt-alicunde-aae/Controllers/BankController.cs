using Microsoft.AspNetCore.Mvc;
using pt_alicunde_aae.Entities;
using pt_alicunde_aae.Utilities;

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

    /// <summary>
    /// Fetches banks from the example API and stores them in the database.
    /// </summary>
    /// <returns>HTTP 200 OK if successful; otherwise, HTTP 500 Internal Server Error with an error message.</returns>
    [HttpPost("fetch")]
    public async Task<IActionResult> FetchAndStoreBanks()
    {
        Result<bool>? result = await _bankService.FetchAndStoreBanksAsync();

        if (result.IsSuccess)
        {
            return Ok("Banks fetched and stored successfully.");
        }

        return StatusCode(500, result.Error);
    }

    /// <summary>
    /// Gets all banks from the database.
    /// </summary>
    /// <returns>A list of all banks if successful; otherwise, HTTP 500 Internal Server Error with an error message.</returns>
    [HttpGet("all")]
    public async Task<IActionResult> GetAllBanks()
    {
        Result<List<Bank>>? result = await _bankService.GetAllBanksAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return StatusCode(500, result.Error);
    }

    /// <summary>
    /// Gets a specific bank by its ID.
    /// </summary>
    /// <param name="id">The ID of the bank to retrieve.</param>
    /// <returns>The requested bank if found; otherwise, HTTP 404 Not Found or HTTP 500 Internal Server Error with an error message.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBankById(int id)
    {
        Result<Bank?>? result = await _bankService.GetBankByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Value == null ? NotFound(result.Error) : StatusCode(500, result.Error);
    }

    /// <summary>
    /// Updates an existing bank.
    /// </summary>
    /// <param name="id">The ID of the bank to update.</param>
    /// <param name="updatedBank">The updated bank data.</param>
    /// <returns>HTTP 200 OK if successful; otherwise, HTTP 404 Not Found or HTTP 500 Internal Server Error with an error message.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBank(int id, [FromBody] Bank updatedBank)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Result<bool>? result = await _bankService.UpdateBankAsync(id, updatedBank);

        if (result.IsSuccess)
        {
            return Ok("Bank updated successfully.");
        }

        return NotFound(result.Error);
    }

    /// <summary>
    /// Deletes a specific bank by its ID.
    /// </summary>
    /// <param name="id">The ID of the bank to delete.</param>
    /// <returns>HTTP 200 OK if successful; otherwise, HTTP 404 Not Found or HTTP 500 Internal Server Error with an error message.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBank(int id)
    {
        Result<bool>? result = await _bankService.DeleteBankAsync(id);

        if (result.IsSuccess)
        {
            return Ok("Bank deleted successfully.");
        }

        return NotFound(result.Error);
    }

    #endregion
}