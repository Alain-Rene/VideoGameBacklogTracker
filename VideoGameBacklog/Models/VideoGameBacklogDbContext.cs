using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VideoGameBacklog.Models;

public partial class VideoGameBacklogDbContext : DbContext
{
    public VideoGameBacklogDbContext()
    {
    }

    public VideoGameBacklogDbContext(DbContextOptions<VideoGameBacklogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<ProgressLog> ProgressLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433; Initial Catalog=VideoGameBacklogDB; User ID=SA; Password=D3ffL6mI9frf; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__games__2AB897DDF13826F8");

            entity.ToTable("games");

            entity.HasIndex(e => e.Id, "UQ__games__3213E83E613C67E9").IsUnique();

            entity.Property(e => e.GameId)
                .ValueGeneratedNever()
                .HasColumnName("GameID");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<ProgressLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__progress__5E5499A8E49403AD");

            entity.ToTable("progressLogs");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.ProgressLogs)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__progressL__GameI__4D94879B");

            entity.HasOne(d => d.User).WithMany(p => p.ProgressLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__progressL__UserI__4CA06362");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F1085D562");

            entity.ToTable("users");

            entity.HasIndex(e => e.GoogleId, "UQ__users__A6FBF31B8DB0D8D2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GoogleId)
                .HasMaxLength(255)
                .HasColumnName("GoogleID");
            entity.Property(e => e.Pfp).HasMaxLength(4000);
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
