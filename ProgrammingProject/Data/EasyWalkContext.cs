
using Microsoft.EntityFrameworkCore;
using ProgrammingProject.Models;

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

            modelBuilder.Entity<WalkingSession>()
            .HasOne(w => w.Walker)
            .WithMany(ws => ws.WalkingSessions)
            .HasForeignKey(ws => ws.WalkerID)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Login>()
            .HasOne(u => u.User)
            .WithOne(lo => lo.Login)
            .HasForeignKey<User>(lo => lo.Id)
            .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Walks>()
           .HasOne(w => w.Walker)
           .WithMany(wa => wa.Walks)
           .HasForeignKey(ws => ws.WalkerId);

            modelBuilder.Entity<Walks>()
           .HasOne(s => s.Suburb)
           .WithMany(wa => wa.Walks)
           .HasForeignKey(wa => wa.Postcode);
           
        }


        public DbSet<Dog> Dogs { get; set; }
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


    }

}
