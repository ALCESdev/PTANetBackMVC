using Moq;
using pt_alicunde_aae.Data;
using pt_alicunde_aae.Entities;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Net;
using Moq.Protected;
using Microsoft.EntityFrameworkCore;
using pt_alicunde_aae.Utilities;

namespace AlicundeAaeUnitTests;

/// <summary>
/// Unit tests for the BankService class.
/// These tests validate the functionality of the BankService methods
/// including fetching, storing, retrieving, updating, and deleting banks.
/// </summary>
public class BankServiceTests
{
    #region VARIABLES

    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<ILogger<BankService>> _loggerMock;
    private readonly BankService _bankService;

    #endregion

    #region CONSTRUCTOR

    /// <summary>
    /// Initializes a new instance of the BankServiceTests class.
    /// Sets up the mocks and the instance of BankService to be tested.
    /// </summary>
    public BankServiceTests()
    {
        DbContextOptions<AppDbContext>? options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        AppDbContext? dbContext = new AppDbContext(options);

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _loggerMock = new Mock<ILogger<BankService>>();

        HttpClient httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        _bankService = new BankService(httpClient, dbContext, _loggerMock.Object);
    }

    #endregion

    #region TESTS

    /// <summary>
    /// Tests that FetchAndStoreBanksAsync returns success when banks are successfully fetched and stored.
    /// </summary>
    [Fact]
    public async Task FetchAndStoreBanksAsync_ReturnsSuccess_WhenBanksAreFetchedAndStored()
    {
        // Arrange
        List<Bank> banks = new List<Bank>
        {
            new Bank
            {
                name = "Test Bank",
                country = "Test Country",
                bic = "TESTBIC123"
            }
        };
        var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(banks));
        jsonContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = jsonContent
            });

        // Act
        Result<bool>? result = await _bankService.FetchAndStoreBanksAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    /// <summary>
    /// Tests that FetchAndStoreBanksAsync returns failure when the API call fails.
    /// </summary>
    [Fact]
    public async Task FetchAndStoreBanksAsync_ReturnsFailure_WhenApiFails()
    {
        // Arrange
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            });

        // Act
        Result<bool>? result = await _bankService.FetchAndStoreBanksAsync();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("An error occurred while fetching or storing banks.", result.Error);
    }

    /// <summary>
    /// Tests that GetAllBanksAsync returns a list of banks when banks exist in the database.
    /// </summary>
    [Fact]
    public async Task GetAllBanksAsync_ReturnsListOfBanks_WhenBanksExistInDatabase()
    {
        // Arrange
        Bank? bank = new Bank
        {
            name = "Test Bank",
            country = "Test Country",
            bic = "TESTBIC123"
        };

        AppDbContext? dbContext = _bankService.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_bankService) as AppDbContext;
        dbContext.Bank.Add(bank);
        await dbContext.SaveChangesAsync();

        // Act
        Result<List<Bank>>? result = await _bankService.GetAllBanksAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value);
        Assert.Equal("Test Bank", result.Value.First().name);
    }

    /// <summary>
    /// Tests that GetBankByIdAsync returns a bank when the bank exists in the database.
    /// </summary>
    [Fact]
    public async Task GetBankByIdAsync_ReturnsBank_WhenBankExists()
    {
        // Arrange
        Bank? bank = new Bank
        {
            name = "Test Bank",
            country = "Test Country",
            bic = "TESTBIC123"
        };

        AppDbContext? dbContext = _bankService.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_bankService) as AppDbContext;
        dbContext.Bank.Add(bank);
        await dbContext.SaveChangesAsync();

        // Act
        Result<Bank?>? result = await _bankService.GetBankByIdAsync(bank.id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Test Bank", result.Value.name);
    }

    /// <summary>
    /// Tests that GetBankByIdAsync returns failure when the bank does not exist in the database.
    /// </summary>
    [Fact]
    public async Task GetBankByIdAsync_ReturnsFailure_WhenBankDoesNotExist()
    {
        // Act
        Result<Bank?>? result = await _bankService.GetBankByIdAsync(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Bank with ID 999 was not found.", result.Error);
    }

    /// <summary>
    /// Tests that UpdateBankAsync returns success when the bank is successfully updated.
    /// </summary>
    [Fact]
    public async Task UpdateBankAsync_ReturnsSuccess_WhenBankIsUpdated()
    {
        // Arrange
        Bank? bank = new Bank
        {
            name = "Original Bank",
            country = "Original Country",
            bic = "ORIGINALBIC"
        };

        AppDbContext? dbContext = _bankService.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_bankService) as AppDbContext;
        dbContext.Bank.Add(bank);
        await dbContext.SaveChangesAsync();

        Bank? updatedBank = new Bank
        {
            name = "Updated Bank",
            country = "Updated Country",
            bic = "UPDATEDBIC"
        };

        // Act
        Result<bool>? result = await _bankService.UpdateBankAsync(bank.id, updatedBank);

        // Assert
        Assert.True(result.IsSuccess);
        Bank? updatedEntity = await dbContext.Bank.FindAsync(bank.id);
        Assert.Equal("Updated Bank", updatedEntity.name);
    }

    /// <summary>
    /// Tests that DeleteBankAsync returns success when the bank is successfully deleted.
    /// </summary>
    [Fact]
    public async Task DeleteBankAsync_ReturnsSuccess_WhenBankIsDeleted()
    {
        // Arrange
        Bank? bank = new Bank
        {
            name = "Test Bank",
            country = "Test Country",
            bic = "TESTBIC123"
        };

        AppDbContext? dbContext = _bankService.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(_bankService) as AppDbContext;
        dbContext.Bank.Add(bank);
        await dbContext.SaveChangesAsync();

        // Act
        Result<bool>? result = await _bankService.DeleteBankAsync(bank.id);

        // Assert
        Assert.True(result.IsSuccess);
        Bank? deletedBank = await dbContext.Bank.FindAsync(bank.id);
        Assert.Null(deletedBank);
    }

    #endregion
}