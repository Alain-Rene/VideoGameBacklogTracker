﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VideoGameBacklog.Models;

#nullable disable

namespace VideoGameBacklog.Migrations
{
    [DbContext(typeof(VideoGameBacklogDbContext))]
    partial class VideoGameBacklogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Friend", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<int>("FriendId")
                        .HasColumnType("int")
                        .HasColumnName("friend_id");

                    b.HasKey("UserId", "FriendId")
                        .HasName("PK__friends__FA44291A0167E3FB");

                    b.HasIndex("FriendId");

                    b.ToTable("friends", (string)null);
                });

            modelBuilder.Entity("VideoGameBacklog.Models.ProgressLog", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LogID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogId"));

                    b.Property<int>("GameId")
                        .HasColumnType("int")
                        .HasColumnName("GameID");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("PlayTime")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("LogId")
                        .HasName("PK__progress__5E5499A80A7B4FF1");

                    b.HasIndex("UserId");

                    b.ToTable("progressLogs", (string)null);
                });

            modelBuilder.Entity("VideoGameBacklog.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GoogleId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("GoogleID");

                    b.Property<int?>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Pfp")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int?>("TotalXp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("TotalXP");

                    b.Property<string>("UserName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__users__3213E83FCF79DA63");

                    b.HasIndex(new[] { "GoogleId" }, "UQ__users__A6FBF31BB4CD516C")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Friend", b =>
                {
                    b.HasOne("VideoGameBacklog.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .IsRequired()
                        .HasConstraintName("FK__friends__friend___08B54D69");

                    b.HasOne("VideoGameBacklog.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__friends__user_id__07C12930");
                });

            modelBuilder.Entity("VideoGameBacklog.Models.ProgressLog", b =>
                {
                    b.HasOne("VideoGameBacklog.Models.User", "User")
                        .WithMany("ProgressLogs")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__progressL__UserI__02FC7413");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VideoGameBacklog.Models.User", b =>
                {
                    b.Navigation("ProgressLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
