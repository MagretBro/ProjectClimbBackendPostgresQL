﻿// <auto-generated />
using System;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241007030922_ClimbingRoutesCreation")]
    partial class ClimbingRoutesCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Models.ClimbingRoute", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BoltCount")
                        .HasColumnType("text");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<string>("Describe")
                        .HasColumnType("text");

                    b.Property<string>("MapPoint")
                        .HasColumnType("text");

                    b.Property<string>("MapVidget")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string[]>("Picture")
                        .HasColumnType("text[]");

                    b.Property<Guid>("SectorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Testimonial")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SectorId");

                    b.ToTable("ClimbingRoutes");
                });

            modelBuilder.Entity("Backend.Models.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Backend.Models.Massive", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Describe")
                        .HasColumnType("text");

                    b.Property<string>("MapPoint")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string[]>("Picture")
                        .HasColumnType("text[]");

                    b.Property<Guid>("RegionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Massives");
                });

            modelBuilder.Entity("Backend.Models.Region", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("Backend.Models.Sector", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Describe")
                        .HasColumnType("text");

                    b.Property<string>("MapPoint")
                        .HasColumnType("text");

                    b.Property<Guid>("MassiveId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string[]>("Picture")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("MassiveId");

                    b.ToTable("Sectors");
                });

            modelBuilder.Entity("Backend.Models.ClimbingRoute", b =>
                {
                    b.HasOne("Backend.Models.Sector", "Sector")
                        .WithMany("ClimbingRoutes")
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sector");
                });

            modelBuilder.Entity("Backend.Models.Massive", b =>
                {
                    b.HasOne("Backend.Models.Region", "Region")
                        .WithMany("Massives")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Backend.Models.Region", b =>
                {
                    b.HasOne("Backend.Models.Country", "Country")
                        .WithMany("Regions")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Backend.Models.Sector", b =>
                {
                    b.HasOne("Backend.Models.Massive", "Massive")
                        .WithMany("Sectors")
                        .HasForeignKey("MassiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Massive");
                });

            modelBuilder.Entity("Backend.Models.Country", b =>
                {
                    b.Navigation("Regions");
                });

            modelBuilder.Entity("Backend.Models.Massive", b =>
                {
                    b.Navigation("Sectors");
                });

            modelBuilder.Entity("Backend.Models.Region", b =>
                {
                    b.Navigation("Massives");
                });

            modelBuilder.Entity("Backend.Models.Sector", b =>
                {
                    b.Navigation("ClimbingRoutes");
                });
#pragma warning restore 612, 618
        }
    }
}
