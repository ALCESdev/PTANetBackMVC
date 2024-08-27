namespace pt_alicunde_aae.Utilities;

/// <summary>
/// Represents the result of an operation that can be either successful or failed.
/// </summary>
/// <typeparam name="T">The type of the value returned if the operation is successful.</typeparam>
public class Result<T>
{
    #region VARIABLES

    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess => Error == null;

    #endregion

    #region CONSTRUCTOR

    /// <summary>
    /// Protected constructor to initialize a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="value">The value resulting from the operation if it was successful.</param>
    /// <param name="error">The error message if the operation failed.</param>
    protected Result(T? value, string? error)
    {
        Value = value;
        Error = error;
    }

    #endregion

    #region METHODS

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="value">The value resulting from the successful operation.</param>
    /// <returns>An instance of <see cref="Result{T}"/> representing a successful result.</returns>
    public static Result<T> Success(T value) => new Result<T>(value, null);

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="error">The error message describing why the operation failed.</param>
    /// <returns>An instance of <see cref="Result{T}"/> representing a failed result.</returns>
    public static Result<T> Failure(string error) => new Result<T>(default, error);

    #endregion
}
