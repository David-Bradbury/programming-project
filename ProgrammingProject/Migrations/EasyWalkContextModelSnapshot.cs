﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProgrammingProject.Data;

#nullable disable

namespace ProgrammingProject.Migrations
{
    [DbContext(typeof(EasyWalkContext))]
    partial class EasyWalkContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DogWalkingSession", b =>
                {
                    b.Property<int>("DogListId")
                        .HasColumnType("int");

                    b.Property<int>("WalkingSessionsSessionID")
                        .HasColumnType("int");

                    b.HasKey("DogListId", "WalkingSessionsSessionID");

                    b.HasIndex("WalkingSessionsSessionID");

                    b.ToTable("DogWalkingSession");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Dog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DogImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DogSize")
                        .HasColumnType("int");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("bit");

                    b.Property<string>("MicrochipNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("OwnerUserId")
                        .HasColumnType("int");

                    b.Property<int>("Temperament")
                        .HasColumnType("int");

                    b.Property<int>("TrainingLevel")
                        .HasColumnType("int");

                    b.Property<int?>("VetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerUserId");

                    b.HasIndex("VetId");

                    b.ToTable("Dogs");
                });

            modelBuilder.Entity("ProgrammingProject.Models.DogRating", b =>
                {
                    b.Property<int>("DogID")
                        .HasColumnType("int");

                    b.Property<int>("WalkerID")
                        .HasColumnType("int");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<DateTime>("RatingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("DogID", "WalkerID");

                    b.HasIndex("WalkerID");

                    b.ToTable("DogRatings");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Login", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.Property<string>("EmailToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Locked")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("char");

                    b.HasKey("Email");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Suburb", b =>
                {
                    b.Property<string>("Postcode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SuburbName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Postcode", "SuburbName");

                    b.ToTable("Suburbs");
                });

            modelBuilder.Entity("ProgrammingProject.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ProgrammingProject.Models.Vet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.Property<string>("PhNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SuburbName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SuburbPostcode")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SuburbPostcode", "SuburbName");

                    b.ToTable("Vets");
                });

            modelBuilder.Entity("ProgrammingProject.Models.WalkerRating", b =>
                {
                    b.Property<int>("WalkerID")
                        .HasColumnType("int");

                    b.Property<int>("OwnerID")
                        .HasColumnType("int");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<DateTime>("RatingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("WalkerID", "OwnerID");

                    b.HasIndex("OwnerID");

                    b.ToTable("WalkerRatings");
                });

            modelBuilder.Entity("ProgrammingProject.Models.WalkingSession", b =>
                {
                    b.Property<int>("SessionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SessionID"));

                    b.Property<DateTime>("ActualEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ActualStartTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ScheduledEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ScheduledStartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("WalkerID")
                        .HasColumnType("int");

                    b.HasKey("SessionID");

                    b.HasIndex("WalkerID");

                    b.ToTable("WalkingSessions");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Walks", b =>
                {
                    b.Property<int>("WalkerId")
                        .HasColumnType("int");

                    b.Property<string>("Postcode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SuburbName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("WalkerId", "Postcode");

                    b.HasIndex("Postcode", "SuburbName");

                    b.ToTable("Walks");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Administrator", b =>
                {
                    b.HasBaseType("ProgrammingProject.Models.User");

                    b.HasDiscriminator().HasValue("Administrator");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Owner", b =>
                {
                    b.HasBaseType("ProgrammingProject.Models.User");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SuburbName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SuburbPostcode")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("SuburbPostcode", "SuburbName");

                    b.HasDiscriminator().HasValue("Owner");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Walker", b =>
                {
                    b.HasBaseType("ProgrammingProject.Models.User");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ExperienceLevel")
                        .HasColumnType("int");

                    b.Property<bool>("IsInsured")
                        .HasColumnType("bit");

                    b.Property<string>("PhNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SuburbName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SuburbPostcode")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("SuburbPostcode", "SuburbName");

                    b.ToTable("User", t =>
                        {
                            t.Property("Country")
                                .HasColumnName("Walker_Country");

                            t.Property("PhNumber")
                                .HasColumnName("Walker_PhNumber");

                            t.Property("State")
                                .HasColumnName("Walker_State");

                            t.Property("StreetAddress")
                                .HasColumnName("Walker_StreetAddress");

                            t.Property("SuburbName")
                                .HasColumnName("Walker_SuburbName");

                            t.Property("SuburbPostcode")
                                .HasColumnName("Walker_SuburbPostcode");
                        });

                    b.HasDiscriminator().HasValue("Walker");
                });

            modelBuilder.Entity("DogWalkingSession", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Dog", null)
                        .WithMany()
                        .HasForeignKey("DogListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProgrammingProject.Models.WalkingSession", null)
                        .WithMany()
                        .HasForeignKey("WalkingSessionsSessionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProgrammingProject.Models.Dog", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Owner", "Owner")
                        .WithMany("Dogs")
                        .HasForeignKey("OwnerUserId");

                    b.HasOne("ProgrammingProject.Models.Vet", "Vet")
                        .WithMany("Dogs")
                        .HasForeignKey("VetId");

                    b.Navigation("Owner");

                    b.Navigation("Vet");
                });

            modelBuilder.Entity("ProgrammingProject.Models.DogRating", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Dog", "Dog")
                        .WithMany("DogRatings")
                        .HasForeignKey("DogID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("ProgrammingProject.Models.Walker", "Walker")
                        .WithMany("DogRatings")
                        .HasForeignKey("WalkerID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Dog");

                    b.Navigation("Walker");
                });

            modelBuilder.Entity("ProgrammingProject.Models.User", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Login", "Login")
                        .WithOne("User")
                        .HasForeignKey("ProgrammingProject.Models.User", "Email")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Login");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Vet", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Suburb", "Suburb")
                        .WithMany()
                        .HasForeignKey("SuburbPostcode", "SuburbName");

                    b.Navigation("Suburb");
                });

            modelBuilder.Entity("ProgrammingProject.Models.WalkerRating", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Owner", "Owner")
                        .WithMany("WalkerRatings")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("ProgrammingProject.Models.Walker", "Walker")
                        .WithMany("WalkerRatings")
                        .HasForeignKey("WalkerID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Walker");
                });

            modelBuilder.Entity("ProgrammingProject.Models.WalkingSession", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Walker", "Walker")
                        .WithMany("WalkingSessions")
                        .HasForeignKey("WalkerID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Walker");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Walks", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Walker", "Walker")
                        .WithMany("Walks")
                        .HasForeignKey("WalkerId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("ProgrammingProject.Models.Suburb", "Suburb")
                        .WithMany("Walks")
                        .HasForeignKey("Postcode", "SuburbName")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Suburb");

                    b.Navigation("Walker");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Owner", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Suburb", "Suburb")
                        .WithMany("Owners")
                        .HasForeignKey("SuburbPostcode", "SuburbName")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Suburb");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Walker", b =>
                {
                    b.HasOne("ProgrammingProject.Models.Suburb", "Suburb")
                        .WithMany("Walkers")
                        .HasForeignKey("SuburbPostcode", "SuburbName")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Suburb");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Dog", b =>
                {
                    b.Navigation("DogRatings");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Login", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Suburb", b =>
                {
                    b.Navigation("Owners");

                    b.Navigation("Walkers");

                    b.Navigation("Walks");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Vet", b =>
                {
                    b.Navigation("Dogs");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Owner", b =>
                {
                    b.Navigation("Dogs");

                    b.Navigation("WalkerRatings");
                });

            modelBuilder.Entity("ProgrammingProject.Models.Walker", b =>
                {
                    b.Navigation("DogRatings");

                    b.Navigation("WalkerRatings");

                    b.Navigation("WalkingSessions");

                    b.Navigation("Walks");
                });
#pragma warning restore 612, 618
        }
    }
}
