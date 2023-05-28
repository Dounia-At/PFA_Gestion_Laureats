using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;
using PFA_Gestion_Laureats.Models;
using PFA_Gestion_Laureats.Services;
using System.Reflection.Metadata;

namespace PFA_Gestion_Laureats.Models
{
    public class MyContext:DbContext
    {
        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<Laureat> Laureats { get; set; }
        public DbSet<AgentDirection> Agents { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<Certificat> Certificat { get; set; }
        public DbSet<ExperiencePro>ExperiencePro{ get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Entreprise> Entreprises { get; set; }
        public DbSet<Annonce> Annonces { get; set; }
        public DbSet<Postulation> Postulations { get; set; }
        public DbSet<Technologie> Technologie { get; set; }
        public DbSet<AnnonceTechnologie> AnnonceTechnologies { get; set; }

        public MyContext(DbContextOptions<MyContext> opt) : base(opt) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnnonceTechnologie>()
            .HasKey("AnnonceId", "TechnologieId");

            modelBuilder.Entity<Utilisateur>()
            .HasIndex(u => u.Login)
            .IsUnique();

            modelBuilder.Entity<Utilisateur>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<Utilisateur>()
              .HasDiscriminator<string>("UtilisateurRole")
              .HasValue<AgentDirection>("AgentDirection")
              .HasValue<Etudiant>("Etudiant")
              .HasValue<Laureat>("Laureat");

           
            modelBuilder.Entity<Message>()
                .HasOne(m => m.UtilisateurReceiver)
                .WithMany(a => a.BoitesReception)
                .HasForeignKey(a => a.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.UtilisateurSender)
                .WithMany(a => a.messagesEnvoyees)
                .HasForeignKey(a => a.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Postulation>()
               .HasOne(p => p.Etudiant)
               .WithMany(e => e.postulations)
               .HasForeignKey(p => p.EtudiantId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
