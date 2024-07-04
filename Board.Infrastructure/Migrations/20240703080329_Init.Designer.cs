﻿// <auto-generated />
using System;
using Board.Domain;
using Board.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Board.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240703080329_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "rating_type", new[] { "increase", "decrease" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Board.Domain.Bulletin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Number"));

                    b.Property<Photo>("Photo")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("photo");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_bulletins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_bulletins_user_id");

                    b.ToTable("bulletins", (string)null);
                });

            modelBuilder.Entity("Board.Domain.Rating", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("BulletinId")
                        .HasColumnType("uuid")
                        .HasColumnName("bulletin_id");

                    b.Property<int>("RatingType")
                        .HasColumnType("integer")
                        .HasColumnName("rating_type");

                    b.HasKey("UserId", "BulletinId")
                        .HasName("pk_ratings");

                    b.HasIndex("BulletinId")
                        .IsUnique()
                        .HasDatabaseName("ix_ratings_bulletin_id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_ratings_user_id");

                    b.ToTable("ratings", (string)null);
                });

            modelBuilder.Entity("Board.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean")
                        .HasColumnName("is_admin");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Board.Domain.Bulletin", b =>
                {
                    b.HasOne("Board.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_bulletins_user_user_id");
                });

            modelBuilder.Entity("Board.Domain.Rating", b =>
                {
                    b.HasOne("Board.Domain.Bulletin", null)
                        .WithOne()
                        .HasForeignKey("Board.Domain.Rating", "BulletinId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ratings_bulletins_bulletin_id");

                    b.HasOne("Board.Domain.User", null)
                        .WithOne()
                        .HasForeignKey("Board.Domain.Rating", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ratings_user_user_id");
                });
#pragma warning restore 612, 618
        }
    }
}
