﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projekt_MVC.Data;

#nullable disable

namespace Projekt_MVC.Migrations
{
    [DbContext(typeof(ForumDB))]
    [Migration("20231204001539_test3")]
    partial class test3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Projekt_MVC.Models.Dyskusja", b =>
                {
                    b.Property<int>("DyskusjaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DyskusjaId"));

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Temat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("DyskusjaId");

                    b.HasIndex("UserId");

                    b.ToTable("dyskusja");
                });

            modelBuilder.Entity("Projekt_MVC.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Projekt_MVC.Models.Dyskusja", b =>
                {
                    b.HasOne("Projekt_MVC.Models.User", "Owner")
                        .WithMany("Dyskusje")
                        .HasForeignKey("UserId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Projekt_MVC.Models.User", b =>
                {
                    b.Navigation("Dyskusje");
                });
#pragma warning restore 612, 618
        }
    }
}
