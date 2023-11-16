﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using IdentityNative.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityNative.Context;

public partial class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public virtual DbSet<GRUPOS> GRUPOS { get; set; }

    public virtual DbSet<GRUPOS_PERMISSOES> GRUPOS_PERMISSOES { get; set; }

    public virtual DbSet<GRUPOS_USUARIOS> GRUPOS_USUARIOS { get; set; }

    public virtual DbSet<PERMISSOES> PERMISSOES { get; set; }

    public virtual DbSet<USUARIOS> USUARIOS { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GRUPOS>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__GRUPOS__3214EC27D3E0B2A4");
        });

        modelBuilder.Entity<GRUPOS_PERMISSOES>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__GRUPOS_P__3214EC27897807D5");

            entity.HasOne(d => d.GRUPO).WithMany(p => p.GRUPOS_PERMISSOES).HasConstraintName("FK_GRUPO_ID");

            entity.HasOne(d => d.PERMISSAO).WithMany(p => p.GRUPOS_PERMISSOES).HasConstraintName("FK_PERMISSAO_ID");
        });

        modelBuilder.Entity<GRUPOS_USUARIOS>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__GRUPOS_U__3214EC27F4C8AB79");

            entity.HasOne(d => d.GRUPO).WithMany(p => p.GRUPOS_USUARIOS).HasConstraintName("FK_GRUPO");

            entity.HasOne(d => d.USUARIO).WithMany(p => p.GRUPOS_USUARIOS).HasConstraintName("FK_USUARIO");
        });

        modelBuilder.Entity<PERMISSOES>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__PERMISSO__3214EC279BBBB057");
        });

        modelBuilder.Entity<USUARIOS>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__USUARIOS__3214EC27CD60026C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}