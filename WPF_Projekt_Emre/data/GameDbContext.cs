using Microsoft.EntityFrameworkCore;
using WPF_Projekt_Emre.models;

namespace WPF_Projekt_Emre.data
{
    public class GameDbContext : DbContext
    {
        public DbSet<Figur> Figuren { get; set; } = null!;
        public DbSet<Field> Fields { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Highscore> Highscores { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=EMRECELEPCI\SQLEXPRESS;Database=Projekt_WPF_Emre;Trusted_Connection=True;TrustServerCertificate=True;"
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasKey(p => p.PlayerID);

            modelBuilder.Entity<Player>()
                .Property(p => p.PlayerName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Player>()
                .Property(p => p.Figur)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Highscore>()
                .HasKey(h => h.HighscoreID);

            modelBuilder.Entity<Highscore>()
                .Property(h => h.PlayerName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Field>()
                .HasKey(f => f.FieldID);

            modelBuilder.Entity<Field>()
                .Property(f => f.FieldName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Field>()
                .Property(f => f.FieldTyp)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Field>()
                .Property(f => f.ImageName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Figur>()
                .HasKey(f => f.FigurID);

            modelBuilder.Entity<Figur>()
                .Property(f => f.FigurName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Figur>()
                .Property(f => f.ImageName)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}