using Microsoft.EntityFrameworkCore;
using Projekt_MVC.Models;

namespace Projekt_MVC.Data
{
    public class ForumDB : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Dyskusja> Dyskusja { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Odpowiedz> Odpowiedz { get; set; }
        public DbSet<Forum> Forum { get; set; }
        public DbSet<Kategoria> Kategoria { get; set; }
        public DbSet<Moderator> Moderator { get; set; }
        public DbSet<Ogloszenie> Ogloszenie { get; set; }
        public DbSet<Ranga> Ranga { get; set; }
        public DbSet<RangaUzytkownika> RangaUzytkownika { get; set; }
        public DbSet<UprawnienieAnonimowych> UprawnienieAnonimowych { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Odpowiedz>()
                    .HasOne(o => o.Autor)
                    .WithMany(u => u.Odpowiedzi)
                    .HasForeignKey(o => o.OdpowiedzId)
                    .OnDelete(DeleteBehavior.Restrict);
        }


    }
}