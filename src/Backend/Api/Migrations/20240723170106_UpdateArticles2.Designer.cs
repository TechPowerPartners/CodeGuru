﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Guard.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240723170106_UpdateArticles2")]
    partial class UpdateArticles2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCorreсt")
                        .HasColumnType("boolean");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("Domain.Entities.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("EditedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("PublichedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string[]>("Tags")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Domain.Entities.Candidate", b =>
                {
                    b.Property<Guid>("VacancyId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<Guid?>("VacancyId1")
                        .HasColumnType("uuid");

                    b.HasKey("VacancyId", "UserId");

                    b.HasIndex("UserId");

                    b.HasIndex("VacancyId1");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("Domain.Entities.Interview", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ComlitionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<Guid>("IntervieweeId")
                        .HasColumnType("uuid");

                    b.Property<int?>("ResultStatus")
                        .HasColumnType("integer");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.ComplexProperty<Dictionary<string, object>>("Date", "Domain.Entities.Interview.Date#InterviewDate", b1 =>
                        {
                            b1.Property<DateTime>("From")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<DateTime>("To")
                                .HasColumnType("timestamp without time zone");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Role", "Domain.Entities.Interview.Role#RoleEnhancement", b1 =>
                        {
                            b1.Property<int>("From")
                                .HasColumnType("integer");

                            b1.Property<int>("To")
                                .HasColumnType("integer");
                        });

                    b.HasKey("Id");

                    b.HasIndex("IntervieweeId");

                    b.ToTable("Interviews");
                });

            modelBuilder.Entity("Domain.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("DifficultyLevel")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfPoints")
                        .HasColumnType("integer");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Domain.Entities.QuestionFiles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("QuestionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionFiles");
                });

            modelBuilder.Entity("Domain.Entities.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeOnly>("TravelTime")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.Vacancy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ClosingDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("LeaderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("LeaderId");

                    b.ToTable("Vacancies");
                });

            modelBuilder.Entity("Domain.Entities.VacancyKeyword", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("VacancyKeywords");
                });

            modelBuilder.Entity("VacancyVacancyKeyword", b =>
                {
                    b.Property<string>("KeywordsValue")
                        .HasColumnType("text");

                    b.Property<Guid>("VacancyId")
                        .HasColumnType("uuid");

                    b.HasKey("KeywordsValue", "VacancyId");

                    b.HasIndex("VacancyId");

                    b.ToTable("VacancyVacancyKeyword");
                });

            modelBuilder.Entity("Domain.Entities.Answer", b =>
                {
                    b.HasOne("Domain.Entities.Question", null)
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Candidate", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Vacancy", "Vacancy")
                        .WithMany()
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Vacancy", null)
                        .WithMany("Candidates")
                        .HasForeignKey("VacancyId1");

                    b.Navigation("User");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("Domain.Entities.Interview", b =>
                {
                    b.HasOne("Domain.Entities.User", "Interviewee")
                        .WithMany()
                        .HasForeignKey("IntervieweeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Interviewee");
                });

            modelBuilder.Entity("Domain.Entities.Question", b =>
                {
                    b.HasOne("Domain.Entities.Test", null)
                        .WithMany("Questions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.QuestionFiles", b =>
                {
                    b.HasOne("Domain.Entities.Question", null)
                        .WithMany("Files")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("Domain.Entities.Vacancy", b =>
                {
                    b.HasOne("Domain.Entities.User", "Leader")
                        .WithMany()
                        .HasForeignKey("LeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Leader");
                });

            modelBuilder.Entity("VacancyVacancyKeyword", b =>
                {
                    b.HasOne("Domain.Entities.VacancyKeyword", null)
                        .WithMany()
                        .HasForeignKey("KeywordsValue")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Vacancy", null)
                        .WithMany()
                        .HasForeignKey("VacancyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Files");
                });

            modelBuilder.Entity("Domain.Entities.Test", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Domain.Entities.Vacancy", b =>
                {
                    b.Navigation("Candidates");
                });
#pragma warning restore 612, 618
        }
    }
}
