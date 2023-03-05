using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Models;

public partial class Tiusr4plMohisatarea4Context : DbContext
{
    public Tiusr4plMohisatarea4Context()
    {
    }

    public Tiusr4plMohisatarea4Context(DbContextOptions<Tiusr4plMohisatarea4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ContactoUsuario> ContactoUsuarios { get; set; }

    public virtual DbSet<Correo> Correos { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosAdministradore> UsuariosAdministradores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("Tarea4Mohisa")
            .UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<ContactoUsuario>(entity =>
        {
            entity.HasKey(e => e.ContactoId);

            entity.ToTable("Contacto_Usuario", "dbo");

            entity.Property(e => e.ContactoId).HasColumnName("ContactoID");
            entity.Property(e => e.Facebook)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Instagram)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Primer_Apellido");
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Segundo_Apellido");
            entity.Property(e => e.Twitter)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ContactoUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contacto_Usuario_Usuario");
        });

        modelBuilder.Entity<Correo>(entity =>
        {
            entity.HasKey(e => e.CorreoId).HasName("PK_Contacto");

            entity.ToTable("Correo", "dbo");

            entity.Property(e => e.CorreoId).HasColumnName("CorreoID");
            entity.Property(e => e.ContactoId).HasColumnName("ContactoID");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.Contacto).WithMany(p => p.Correos)
                .HasForeignKey(d => d.ContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contacto_Contacto_Usuario");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estado", "dbo");

            entity.Property(e => e.EstadoId)
                .ValueGeneratedNever()
                .HasColumnName("EstadoID");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.ToTable("Telefono", "dbo");

            entity.Property(e => e.TelefonoId).HasColumnName("TelefonoID");
            entity.Property(e => e.ContactoId).HasColumnName("ContactoID");
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Contacto).WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.ContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_Contacto_Usuario");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.IdToken);

            entity.Property(e => e.IdToken).HasColumnName("idToken");
            entity.Property(e => e.Token1).HasColumnName("token");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Identificacion).HasName("PK_Usuario");

            entity.ToTable("Usuarios", "dbo");

            entity.Property(e => e.Identificacion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.IdToken).HasColumnName("idToken");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Primer_Apellido");
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Segundo_Apellido");

            entity.HasOne(d => d.Estado).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Estado");

            entity.HasOne(d => d.IdTokenNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdToken)
                .HasConstraintName("Usuario_Token");
        });

        modelBuilder.Entity<UsuariosAdministradore>(entity =>
        {
            entity.HasKey(e => e.CodigoUsuario);

            entity.ToTable("Usuarios_Administradores", "dbo");

            entity.Property(e => e.CodigoUsuario)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigoUsuario");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.IdToken).HasColumnName("idToken");

            entity.HasOne(d => d.Estado).WithMany(p => p.UsuariosAdministradores)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Administradores_Estado");

            entity.HasOne(d => d.IdTokenNavigation).WithMany(p => p.UsuariosAdministradores)
                .HasForeignKey(d => d.IdToken)
                .HasConstraintName("UsuarioAdmin_Token");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
