using System;

namespace Entities.DTO;
//this data transfer object need for post request
//in other words it accepting input data from POST http request
public class EmployeeForCreationDto
{
	public string Name {get; set;}
	public int Age {get; set;}
	public string Position {get; set;}
}