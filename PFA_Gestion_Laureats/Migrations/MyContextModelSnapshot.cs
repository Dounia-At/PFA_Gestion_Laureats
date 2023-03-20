﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PFA_Gestion_Laureats.Models;

#nullable disable

namespace PFA_Gestion_Laureats.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Annonce", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date_Creation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_limite_Deposer")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email_Reception")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UtilisateurId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UtilisateurId");

                    b.ToTable("Annonces");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Certificat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date_Emission")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Expiration")
                        .HasColumnType("datetime2");

                    b.Property<int>("EtudiantId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organisation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EtudiantId");

                    b.ToTable("Certificat");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Entreprise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Convention")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pays")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ville")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Entreprises");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.ExperiencePro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EntrepriseId")
                        .HasColumnType("int");

                    b.Property<bool>("Etat")
                        .HasColumnType("bit");

                    b.Property<int>("EtudiantId")
                        .HasColumnType("int");

                    b.Property<string>("Post")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type_Emploi")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntrepriseId");

                    b.HasIndex("EtudiantId");

                    b.ToTable("ExperiencePro");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Formation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date_Debut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Fin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Diplome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ecole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EtudiantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EtudiantId");

                    b.ToTable("Formations");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date_Envoie")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("contenu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Postulation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnnonceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date_Consultation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Date_Postulation")
                        .HasColumnType("datetime2");

                    b.Property<int>("EtudiantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AnnonceId");

                    b.HasIndex("EtudiantId");

                    b.ToTable("Postulations");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Projet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date_Debut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Fin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EtudiantId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EtudiantId");

                    b.ToTable("Projets");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Stage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date_Debut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date_Fin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EntrepriseId")
                        .HasColumnType("int");

                    b.Property<int>("EtudiantId")
                        .HasColumnType("int");

                    b.Property<string>("Sujet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EntrepriseId");

                    b.HasIndex("EtudiantId");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AgentDirectionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date_Test")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EntrepriseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Heure_Test")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AgentDirectionId");

                    b.HasIndex("EntrepriseId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Utilisateur", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EtudiantRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Isvalide")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo_Profil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titre_Profil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UtilisateurRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Utilisateurs");

                    b.HasDiscriminator<string>("UtilisateurRole").HasValue("Utilisateur");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.AgentDirection", b =>
                {
                    b.HasBaseType("PFA_Gestion_Laureats.Models.Utilisateur");

                    b.HasDiscriminator().HasValue("Agent");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Etudiant", b =>
                {
                    b.HasBaseType("PFA_Gestion_Laureats.Models.Utilisateur");

                    b.Property<DateTime>("date_Inscriptionion")
                        .HasColumnType("datetime2");

                    b.Property<string>("specialite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Etudiant");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Laureat", b =>
                {
                    b.HasBaseType("PFA_Gestion_Laureats.Models.Etudiant");

                    b.Property<DateTime>("Date_Fin_Etude")
                        .HasColumnType("datetime2");

                    b.HasDiscriminator().HasValue("Laureat");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Annonce", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Utilisateur", "utilisateur")
                        .WithMany("annonces")
                        .HasForeignKey("UtilisateurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("utilisateur");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Certificat", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Etudiant", "Etudiant")
                        .WithMany("certificats")
                        .HasForeignKey("EtudiantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etudiant");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.ExperiencePro", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Entreprise", "entreprise")
                        .WithMany("experiences")
                        .HasForeignKey("EntrepriseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PFA_Gestion_Laureats.Models.Etudiant", "Etudiant")
                        .WithMany("experiences")
                        .HasForeignKey("EtudiantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etudiant");

                    b.Navigation("entreprise");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Formation", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Etudiant", "Etudiant")
                        .WithMany("formations")
                        .HasForeignKey("EtudiantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etudiant");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Message", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Utilisateur", "UtilisateurReceiver")
                        .WithMany("BoitesReception")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PFA_Gestion_Laureats.Models.Utilisateur", "UtilisateurSender")
                        .WithMany("messagesEnvoyees")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UtilisateurReceiver");

                    b.Navigation("UtilisateurSender");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Postulation", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Annonce", "Annonce")
                        .WithMany("postulations")
                        .HasForeignKey("AnnonceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PFA_Gestion_Laureats.Models.Etudiant", "Etudiant")
                        .WithMany("postulations")
                        .HasForeignKey("EtudiantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Annonce");

                    b.Navigation("Etudiant");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Projet", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Etudiant", "Etudiant")
                        .WithMany("projets")
                        .HasForeignKey("EtudiantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etudiant");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Stage", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.Entreprise", "entreprise")
                        .WithMany("stages")
                        .HasForeignKey("EntrepriseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PFA_Gestion_Laureats.Models.Etudiant", "Etudiant")
                        .WithMany("stages")
                        .HasForeignKey("EtudiantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Etudiant");

                    b.Navigation("entreprise");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Test", b =>
                {
                    b.HasOne("PFA_Gestion_Laureats.Models.AgentDirection", "agentDirection")
                        .WithMany("tests")
                        .HasForeignKey("AgentDirectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PFA_Gestion_Laureats.Models.Entreprise", "entreprise")
                        .WithMany("tests")
                        .HasForeignKey("EntrepriseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("agentDirection");

                    b.Navigation("entreprise");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Annonce", b =>
                {
                    b.Navigation("postulations");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Entreprise", b =>
                {
                    b.Navigation("experiences");

                    b.Navigation("stages");

                    b.Navigation("tests");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Utilisateur", b =>
                {
                    b.Navigation("BoitesReception");

                    b.Navigation("annonces");

                    b.Navigation("messagesEnvoyees");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.AgentDirection", b =>
                {
                    b.Navigation("tests");
                });

            modelBuilder.Entity("PFA_Gestion_Laureats.Models.Etudiant", b =>
                {
                    b.Navigation("certificats");

                    b.Navigation("experiences");

                    b.Navigation("formations");

                    b.Navigation("postulations");

                    b.Navigation("projets");

                    b.Navigation("stages");
                });
#pragma warning restore 612, 618
        }
    }
}
