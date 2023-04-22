using System;

namespace Entities.DTO;
//this data transfer object need for post request
//in other words it accepting input data from POST http request
public class CompanyForCreationDto
{
	public string Name {get; set;}
	public string Address {get; set;}
	public string Country {get; set;}
}