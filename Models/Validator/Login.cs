
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Login 
{
    [Key,ForeignKey("User")]
    public int? UserId { get; set; }
    public string? Email { get; set;}
    public string? Password { get; set;} 
    public string? ConfirmPassword { get; set;} 
}

