﻿// <auto-generated />
using System;
using Catering.Infrastructure.Mailing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catering.Infrastructure.Mailing.Migrations
{
    [DbContext(typeof(MailingDbContext))]
    [Migration("20220722123152_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("mailing")
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Catering.Application.Mailing.Emails.EmailTemplate", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("HtmlTemplate")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Templates", "mailing");
                });

            modelBuilder.Entity("Catering.Application.Mailing.Emails.FailedCateringEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("GeneratedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Recepiants")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FailedEmails", "mailing");
                });
#pragma warning restore 612, 618
        }
    }
}