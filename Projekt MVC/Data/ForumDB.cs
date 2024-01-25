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

            modelBuilder.Entity<Dyskusja>()
                .HasOne(m => m.Wlasciciel)
                .WithMany(u => u.Dyskusje)
                .HasForeignKey(u => u.DyskusjaId)
                .IsRequired();

            modelBuilder.Entity<Moderator>()
                .HasOne(m => m.Uzytkownik)
                .WithMany(u => u.Moderatorzy)
                .HasForeignKey(m => m.IdUzytkownika)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Odpowiedzi)
                .WithOne(o => o.Autor)
                .HasForeignKey(o => o.OdpowiedzId)
                .OnDelete(DeleteBehavior.NoAction);
        }


    }


}