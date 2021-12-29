﻿// <auto-generated />
using System;
using Backened.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backened.Migrations
{
    [DbContext(typeof(SignalRChatContext))]
    partial class SignalRChatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backened.Models.Chat", b =>
                {
                    b.Property<long>("chatId")
                        .HasColumnType("bigint");

                    b.Property<string>("connectionId")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool?>("isPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("messageDate")
                        .HasColumnType("datetime");

                    b.Property<string>("messageStatus")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("receiverId")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("senderId")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("chatId");

                    b.HasIndex("senderId", "receiverId")
                        .HasName("NonClusteredIndex-20210902-114105");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("Backened.Models.Login", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("userName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("userPassword")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("userId");

                    b.HasIndex("userName")
                        .IsUnique()
                        .HasFilter("[userName] IS NOT NULL");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("Backened.Models.Signup", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("loginStatus")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("userEmail")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("userName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("userPassword")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("userId");

                    b.HasIndex("userEmail")
                        .IsUnique()
                        .HasFilter("[userEmail] IS NOT NULL");

                    b.HasIndex("userName")
                        .IsUnique()
                        .HasFilter("[userName] IS NOT NULL");

                    b.ToTable("Signups");
                });
#pragma warning restore 612, 618
        }
    }
}
