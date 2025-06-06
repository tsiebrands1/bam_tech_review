﻿namespace Stargate.Data.Entities;

using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class StargateContextConfiguration(DbContextOptions<StargateContext> options) 
	: StargateContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(StargateContext).Assembly);

		//SeedData(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	private static void SeedData(ModelBuilder modelBuilder)
	{
		//add seed data
		_ = modelBuilder.Entity<Person>()
			.HasData(
				new Person
				{
					Id = 1,
					Name = "John Doe"
				},
				new Person
				{
					Id = 2,
					Name = "Jane Doe"
				}
			);

		modelBuilder.Entity<AstronautDetail>()
			.HasData(
				new AstronautDetail
				{
					Id = 1,
					PersonId = 1,
					CurrentRank = "1LT",
					CurrentDutyTitle = "Commander",
					CareerStartDate = DateTime.Now
				}
			);

		modelBuilder.Entity<AstronautDuty>()
			.HasData(
				new AstronautDuty
				{
					Id = 1,
					PersonId = 1,
					DutyStartDate = DateTime.Now,
					DutyTitle = "Commander",
					Rank = "1LT"
				}
			);
	}
}
