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

    public virtual DbSet<ProgressLog> ProgressLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433; Initial Catalog=VideoGameBacklogDB; User ID=SA; Password=D3ffL6mI9frf; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProgressLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__progress__5E5499A8295AB690");

            entity.ToTable("progressLogs");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ProgressLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__progressL__UserI__06CD04F7");
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
            entity.Property(e => e.Level).HasDefaultValue(1);
            entity.Property(e => e.Pfp).HasMaxLength(4000);
            entity.Property(e => e.TotalXp)
                .HasDefaultValue(0)
                .HasColumnName("TotalXP");
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasMany(d => d.Friends).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Friend",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__friends__friend___03F0984C"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__friends__user_id__02FC7413"),
                    j =>
                    {
                        j.HasKey("UserId", "FriendId").HasName("PK__friends__FA44291A089BBCBC");
                        j.ToTable("friends");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("FriendId").HasColumnName("friend_id");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Friends)
                .UsingEntity<Dictionary<string, object>>(
                    "Friend",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__friends__user_id__02FC7413"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__friends__friend___03F0984C"),
                    j =>
                    {
                        j.HasKey("UserId", "FriendId").HasName("PK__friends__FA44291A089BBCBC");
                        j.ToTable("friends");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("FriendId").HasColumnName("friend_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
