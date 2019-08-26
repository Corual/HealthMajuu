﻿// <auto-generated />
using System;
using ManjuuInfrastructure.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ManjuuInfrastructure.Migrations
{
    [DbContext(typeof(HealthManjuuCoreContext))]
    [Migration("20190826125610_HealthManjuuCore")]
    partial class HealthManjuuCore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("ManjuuInfrastructure.Repository.Entity.JobConfiguration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("PingSendCount");

                    b.Property<int>("PresetTimeout");

                    b.Property<DateTime>("StartToWrokTime");

                    b.Property<int>("State");

                    b.Property<DateTime>("StopToWorkTime");

                    b.Property<int?>("WorkSpan");

                    b.HasKey("Id");

                    b.ToTable("JobConfigurations");
                });
#pragma warning restore 612, 618
        }
    }
}
