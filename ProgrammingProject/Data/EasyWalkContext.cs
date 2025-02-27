﻿using ProgrammingProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ProgrammingProject.Data
{
    public class EasyWalkContext : DbContext
    {

        public EasyWalkContext(DbContextOptions<EasyWalkContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DogRating>()
           .HasOne(d => d.Dog)
           .WithMany(dr => dr.DogRatings)
           .HasForeignKey(dr => dr.DogID)
           .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<DogRating>()
            .HasOne(w => w.Walker)
            .WithMany(dr => dr.DogRatings)
            .HasForeignKey(dr => dr.WalkerID)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<DogRating>()
            .HasKey(dr => new
            {
                dr.DogID,
                dr.WalkerID
            });

            modelBuilder.Entity<WalkerRating>()
           .HasOne(o => o.Owner)
           .WithMany(wr => wr.WalkerRatings)
           .HasForeignKey(wr => wr.OwnerID)
           .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<WalkerRating>()
            .HasOne(w => w.Walker)
            .WithMany(wr => wr.WalkerRatings)
            .HasForeignKey(wr => wr.WalkerID)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<WalkerRating>()
            .HasKey(wr => new
            {
                wr.WalkerID,
                wr.OwnerID
            });

            modelBuilder.Entity<WalkingSession>()
            .HasOne(w => w.Walker)
            .WithMany(ws => ws.WalkingSessions)
            .HasForeignKey(ws => ws.WalkerID)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<User>()
            .HasOne(l => l.Login)
            .WithOne(u => u.User)
            .HasForeignKey<User>(u => u.Email)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Walks>()
           .HasOne(w => w.Walker)
           .WithMany(wa => wa.Walks)
           .HasForeignKey(ws => ws.WalkerId)
           .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Walks>()
           .HasOne(s => s.Suburb)
           .WithMany(wa => wa.Walks)
           .HasForeignKey(wa => new
           {
               wa.Postcode,
               wa.SuburbName,
               wa.State
           })
           .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Walks>()
            .HasKey(w => new
            {
                w.WalkerId,
                w.Postcode
            });

            modelBuilder.Entity<Walker>()
            .HasOne(s => s.Suburb)
            .WithMany(w => w.Walkers)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Owner>()
            .HasOne(s => s.Suburb)
            .WithMany(o => o.Owners)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Suburb>()
                .HasKey(s => new
                {
                    s.Postcode,
                    s.SuburbName,
                    s.State
                });

            modelBuilder.Entity<Dog>()
                .HasOne(b => b.Breed)
                .WithMany(d => d.Dogs)
                //.HasForeignKey(b => b.BreedId)
                .OnDelete(DeleteBehavior.ClientCascade);

        }


        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Walker> Walkers { get; set; }
        public DbSet<WalkingSession> WalkingSessions { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Administrator> Admins { get; set; }
        public DbSet<Walks> Walks { get; set; }
        public DbSet<DogRating> DogRatings { get; set; }
        public DbSet<WalkerRating> WalkerRatings { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Suburb> Suburbs { get; set; }



    }

}
