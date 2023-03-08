using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Tarea4.Models;

public partial class Tiusr11plMohisatarea4Context : DbContext
{
    public Tiusr11plMohisatarea4Context()
    {
    }

    public Tiusr11plMohisatarea4Context(DbContextOptions<Tiusr11plMohisatarea4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ContactoUsuario> ContactoUsuarios { get; set; }

    public virtual DbSet<Correo> Correos { get; set; }

    public virtual DbSet<DuracionToken> DuracionTokens { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuariosAdministradore> UsuariosAdministradores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tiusr11pl.cuc-carrera-ti.ac.cr.\\MSSQLSERVER2019;Database=tiusr11pl_MOHISATarea4;user id=MOHISA; password=%30ll6dI1; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("MOHISA")
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

        modelBuilder.Entity<DuracionToken>(entity =>
        {
            entity.HasKey(e => e.DuracionId);

            entity.ToTable("Duracion_Token", "dbo");

            entity.Property(e => e.DuracionId).HasColumnName("DuracionID");
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
            entity.HasKey(e => e.TokenId).HasName("PK_Tokens");

            entity.ToTable("Token", "dbo");

            entity.Property(e => e.TokenId).HasColumnName("TokenID");
            entity.Property(e => e.DuracionId).HasColumnName("DuracionID");
            entity.Property(e => e.FechaSolicitud).HasColumnType("datetime");
            entity.Property(e => e.Token1)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Token");
            entity.Property(e => e.UsuarioId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("UsuarioID");

            entity.HasOne(d => d.Duracion).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.DuracionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tokens_Duracion_Token");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_Usuarios_Administradores");

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_Usuarios");
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

            entity.HasOne(d => d.Estado).WithMany(p => p.UsuariosAdministradores)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Administradores_Estado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
