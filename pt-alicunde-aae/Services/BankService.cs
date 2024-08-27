using Microsoft.EntityFrameworkCore;
using pt_alicunde_aae.Data;
using pt_alicunde_aae.Entities;
using pt_alicunde_aae.Utilities;

public class BankService
{
    #region VARIABLES

    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly ILogger<BankService> _logger;
    private const string BanksApiUrl = "https://api.opendata.esett.com/EXP06/Banks";

    #endregion

    #region CONSTRUCTOR

    public BankService(
        HttpClient httpClient,
        AppDbContext context,
        ILogger<BankService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Fetches and stores banks from the example API.
    /// </summary>
    /// <returns>
    /// A <see cref="Result{T}"/> object containing a boolean indicating whether 
    /// the operation was successful. If failed, contains an error message.
    /// </returns>
    public async Task<Result<bool>> FetchAndStoreBanksAsync()
    {
        try
        {
            List<Bank>? banksList = await _httpClient.GetFromJsonAsync<List<Bank>>(BanksApiUrl);

            if (banksList == null || banksList.Count == 0)
            {
                _logger.LogWarning("No banks were returned from the API.");
                return Result<bool>.Failure("No banks were returned from the API.");
            }

            _context.Bank.AddRange(banksList);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching or storing banks.");
            return Result<bool>.Failure("An error occurred while fetching or storing banks.");
        }
    }

    /// <summary>
    /// Gets all banks from the database.
    /// </summary>
    /// <returns>
    /// A <see cref="Result{T}"/> object containing a list of all banks if successful.
    /// If failed, contains an error message.
    /// </returns>
    public async Task<Result<List<Bank>>> GetAllBanksAsync()
    {
        try
        {
            List<Bank>? banksList = await _context.Bank.ToListAsync();
            return Result<List<Bank>>.Success(banksList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the banks.");
            return Result<List<Bank>>.Failure("An error occurred while retrieving the banks.");
        }
    }

    /// <summary>
    /// Gets a specific bank by its ID.
    /// </summary>
    /// <param name="id">The ID of the bank to retrieve.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> object containing the requested bank if found.
    /// If not found or failed, contains an error message.
    /// </returns>
    public async Task<Result<Bank?>> GetBankByIdAsync(int id)
    {
        try
        {
            Bank? bank = await _context.Bank.FindAsync(id);

            if (bank == null)
            {
                return Result<Bank?>.Failure($"Bank with ID {id} was not found.");
            }

            return Result<Bank?>.Success(bank);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the bank.");
            return Result<Bank?>.Failure("An error occurred while retrieving the bank.");
        }
    }

    /// <summary>
    /// Updates an existing bank in the database.
    /// </summary>
    /// <param name="id">The ID of the bank to update.</param>
    /// <param name="updatedBank">The updated bank data.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> object containing a boolean indicating whether 
    /// the update was successful. If failed, contains an error message.
    /// </returns>
    public async Task<Result<bool>> UpdateBankAsync(int id, Bank updatedBank)
    {
        try
        {
            Bank? existingBank = await _context.Bank.FindAsync(id);

            if (existingBank == null)
            {
                return Result<bool>.Failure($"Bank with ID {id} was not found.");
            }

            MapBank(existingBank, updatedBank);

            _context.Bank.Update(existingBank);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the bank.");
            return Result<bool>.Failure("An error occurred while updating the bank.");
        }
    }

    /// <summary>
    /// Deletes a specific bank from the database.
    /// </summary>
    /// <param name="id">The ID of the bank to delete.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> object containing a boolean indicating whether 
    /// the deletion was successful. If failed, contains an error message.
    /// </returns>
    public async Task<Result<bool>> DeleteBankAsync(int id)
    {
        try
        {
            Bank? bank = await _context.Bank.FindAsync(id);

            if (bank == null)
            {
                return Result<bool>.Failure($"Bank with ID {id} was not found.");
            }

            _context.Bank.Remove(bank);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the bank.");
            return Result<bool>.Failure("An error occurred while deleting the bank.");
        }
    }

    #endregion

    #region PRIVATE METHODS

    /// <summary>
    /// Maps the properties of the source Bank to the target Bank.
    /// </summary>
    /// <param name="targetBank">The bank entity to be updated.</param>
    /// <param name="sourceBank">The bank entity containing updated data.</param>
    private void MapBank(Bank targetBank, Bank sourceBank)
    {
        targetBank.bic = sourceBank.bic;
        targetBank.country = sourceBank.country;
        targetBank.name = sourceBank.name;
    }

    #endregion
}