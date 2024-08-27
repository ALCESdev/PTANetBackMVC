using Microsoft.EntityFrameworkCore;
using pt_alicunde_aae.Data;
using pt_alicunde_aae.Entities;

public class BankService
{
    #region VARIABLES

    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly ILogger<BankService> _logger;

    #endregion

    #region CONSTRUCTOR

    public BankService(HttpClient httpClient, AppDbContext context, ILogger<BankService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Fetches and stores banks from example API.
    /// </summary>
    /// <returns>A boolean indicating whether the operation was successful or not.</returns>
    public async Task<bool> FetchAndStoreBanksAsync()
    {
        try
        {
            List<Bank>? banks = await _httpClient.GetFromJsonAsync<List<Bank>>("https://api.opendata.esett.com/EXP06/Banks");

            if (banks == null || banks.Count == 0)
            {
                _logger.LogWarning("No banks were returned from the API.");
                return false;
            }

            _context.Bank.AddRange(banks);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching or storing banks.");
            return false;
        }
    }

    /// <summary>
    /// Gets all banks from the database.
    /// </summary>
    /// <returns>A list of all banks.</returns>
    public async Task<List<Bank>> GetAllBanksAsync() => await _context.Bank.ToListAsync();

    /// <summary>
    /// Gets a specific bank by its id.
    /// </summary>
    /// <param name="id">The bank's id.</param>
    /// <returns>The requested bank if found; otherwise, null.</returns>
    public async Task<Bank?> GetBankByIdAsync(int id) => await _context.Bank.FindAsync(id);

    /// <summary>
    /// Updates an existing bank in the database.
    /// </summary>
    /// <param name="id">The bank's id to update.</param>
    /// <param name="updatedBank">The updated bank data.</param>
    /// <returns>A boolean indicating whether the update was successful.</returns>
    public async Task<bool> UpdateBankAsync(int id, Bank updatedBank)
    {
        Bank? existingBank = await _context.Bank.FindAsync(id);
        if (existingBank == null)
        {
            return false;
        }

        existingBank.bic = updatedBank.bic;
        existingBank.country = updatedBank.country;
        existingBank.name = updatedBank.name;

        _context.Bank.Update(existingBank);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Deletes a specific bank from the database.
    /// </summary>
    /// <param name="id">The bank's id to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    public async Task<bool> DeleteBankAsync(int id)
    {
        Bank? bank = await _context.Bank.FindAsync(id);
        if (bank == null)
        {
            return false;
        }

        _context.Bank.Remove(bank);
        await _context.SaveChangesAsync();
        return true;
    }

    #endregion
}
