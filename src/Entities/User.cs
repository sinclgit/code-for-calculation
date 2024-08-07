using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities{

public class User
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
    public string Email { get; set; }
    //public Role Role { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }
}
}