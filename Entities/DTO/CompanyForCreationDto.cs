using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DTO;
//this data transfer object need for post request
//in other words it accepting input data from POST http request
public class CompanyForCreationDto
{
    [Required(ErrorMessage = "Company name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the company name is 30 characters.")]
	public string Name {get; set;}

    [Required(ErrorMessage = "Company address is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the company address is 30 characters.")]
	public string Address {get; set;}

    [Required(ErrorMessage = "Company country is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the company country is 30 characters.")]
	public string Country {get; set;}
}
