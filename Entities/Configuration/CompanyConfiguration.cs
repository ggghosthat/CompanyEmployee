using Entities.Models;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Configuration;
//Here we are populating company instances
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
	public void Configure(EntityTypeBuilder<Company> builder)
	{
		builder.HasData
		(
			new Company
			{
				Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
				Name = "Canonical",
				Address = "Holly Address",
				Country = "USA"
			},
			new Company
			{
				Id = new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"),
				Name = "LTE",
				Address = "Holly Address2",
				Country = "USA"
			}
		);
	}
}