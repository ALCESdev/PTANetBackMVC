using System.ComponentModel.DataAnnotations;

namespace pt_alicunde_aae.Entities;

public class Bank
{
    public int id { get; set; }

    [Required(ErrorMessage = "BIC is required.")]
    [StringLength(11, ErrorMessage = "BIC cannot be longer than 11 characters.")]
    public string bic { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    public string country { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string name { get; set; }
}