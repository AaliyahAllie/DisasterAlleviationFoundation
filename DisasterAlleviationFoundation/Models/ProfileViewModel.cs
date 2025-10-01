using System.ComponentModel.DataAnnotations;

public class ProfileViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; }
}
