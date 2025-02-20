
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Address
{
    [Key]
    public int? ID { get; set; }

    [ForeignKey("User")]
    public int? UserId {get;set;} 
    public string? AddressOne { get; set;} 

    public string? AddressTwo { get; set;}   

    public string? Country { get; set;}  

    public string? City { get; set;}

    public string? State { get; set;}    
    public string? Code { get; set;}  
}
