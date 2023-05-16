using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DTO;
//this data transfer object need for post request
//in other words it accepting input data from POST http request
public class EmployeeForCreationDto : EmployeeForManipulationDto
{
}
