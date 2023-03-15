using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;

namespace PFA_Gestion_Laureats.Models
{
    public class MyContext:DbContext
    {
        public DbSet<Etudiant> Etudiants { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Entreprise> Entreprises { get; set; }
        public DbSet<Annonce> Annonces { get; set; }
        public DbSet<Postulation> Postulations { get; set; }
        public MyContext(DbContextOptions<MyContext> opt) : base(opt) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Utilisateur>().UseTpcMappingStrategy();
            modelBuilder.Entity<Etudiant>().UseTpcMappingStrategy();

        }
    }
}
