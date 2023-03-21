
using Microsoft.EntityFrameworkCore;
using ProgrammingProject.Models;

namespace ProgrammingProject.Data
{
    public class EasyWalkContext : DbContext
    {

        public EasyWalkContext(DbContextOptions<EasyWalkContext> options) : base(options)
        { }

        public DbSet<Dog> Dogs { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Walker> Walkers { get; set; }
        public DbSet<WalkingSession> WalkingSessions { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Administrator> Admins { get; set; }


    }
}
