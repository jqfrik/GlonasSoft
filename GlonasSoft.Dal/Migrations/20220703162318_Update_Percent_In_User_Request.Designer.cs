﻿// <auto-generated />
using System;
using GlonasSoft.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GlonasSoft.Dal.Migrations
{
    [DbContext(typeof(GlonasContext))]
    [Migration("20220703162318_Update_Percent_In_User_Request")]
    partial class Update_Percent_In_User_Request
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.5.22302.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GlonasSoft.Dal.Entities.ResultDal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Count_Sign_In")
                        .HasColumnType("bigint");

                    b.Property<Guid>("User_Id")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("ResultDal");
                });

            modelBuilder.Entity("GlonasSoft.Dal.Entities.UserDal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Sign_In_Count")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GlonasSoft.Dal.Entities.UserRequestDal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Percent")
                        .HasColumnType("double precision");

                    b.Property<Guid>("Query")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ResultId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserDalId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ResultId");

                    b.HasIndex("UserDalId");

                    b.ToTable("UserRequests");
                });

            modelBuilder.Entity("GlonasSoft.Dal.Entities.UserRequestDal", b =>
                {
                    b.HasOne("GlonasSoft.Dal.Entities.ResultDal", "Result")
                        .WithMany()
                        .HasForeignKey("ResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GlonasSoft.Dal.Entities.UserDal", null)
                        .WithMany("Requests")
                        .HasForeignKey("UserDalId");

                    b.Navigation("Result");
                });

            modelBuilder.Entity("GlonasSoft.Dal.Entities.UserDal", b =>
                {
                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
