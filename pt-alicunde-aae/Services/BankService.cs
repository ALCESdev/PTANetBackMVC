using pt_alicunde_aae.Data;
using pt_alicunde_aae.Entities;

public class BankService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context;
    private readonly ILogger<BankService> _logger;

    public BankService(HttpClient httpClient, AppDbContext context, ILogger<BankService> logger)
    {
        _httpClient = httpClient;
        _context = context;
        _logger = logger;
    }

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
}
