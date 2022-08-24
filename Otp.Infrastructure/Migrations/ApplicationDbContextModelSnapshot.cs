﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Otp.Infrastructure.Persistence;

#nullable disable

namespace Otp.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Otp.Core.Domains.Entities.App", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CallbackUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("EndpointSecret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HashedApiKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("PrincipalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PrincipalId");

                    b.HasIndex("Name", "PrincipalId")
                        .IsUnique();

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.CallbackEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusCode")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("CallbackEvents");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.ChannelPrice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Threshold")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("ChannelPrices");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.Discount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelPriceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PrincipalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelPriceId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PrincipalId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.OtpRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuthenticityKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Availability")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CancelUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Channel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorrelationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxAttempts")
                        .HasColumnType("int");

                    b.Property<string>("Recipient")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResendCount")
                        .HasColumnType("int");

                    b.Property<string>("SuccessUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("CreatedAt", "Id");

                    b.ToTable("OtpRequests");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.Principal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nchar(36)")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Principals");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.App", b =>
                {
                    b.HasOne("Otp.Core.Domains.Entities.Principal", "Principal")
                        .WithMany()
                        .HasForeignKey("PrincipalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Otp.Core.Domains.ValueObjects.Branding", "Branding", b1 =>
                        {
                            b1.Property<Guid>("AppId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("BackgroundUrl")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("LogoUrl")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("SmsMessageTemplate")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AppId");

                            b1.ToTable("Apps");

                            b1.WithOwner()
                                .HasForeignKey("AppId");
                        });

                    b.Navigation("Branding")
                        .IsRequired();

                    b.Navigation("Principal");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.CallbackEvent", b =>
                {
                    b.HasOne("Otp.Core.Domains.Entities.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.Discount", b =>
                {
                    b.HasOne("Otp.Core.Domains.Entities.ChannelPrice", "ChannelPrice")
                        .WithMany()
                        .HasForeignKey("ChannelPriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Otp.Core.Domains.Entities.Principal", "Principal")
                        .WithMany()
                        .HasForeignKey("PrincipalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelPrice");

                    b.Navigation("Principal");
                });

            modelBuilder.Entity("Otp.Core.Domains.Entities.OtpRequest", b =>
                {
                    b.HasOne("Otp.Core.Domains.Entities.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Otp.Core.Domains.ValueObjects.ClientInfo", "ClientInfo", b1 =>
                        {
                            b1.Property<Guid>("OtpRequestId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("IpAddress")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Referrer")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("UserAgent")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("OtpRequestId");

                            b1.ToTable("OtpRequests");

                            b1.WithOwner()
                                .HasForeignKey("OtpRequestId");
                        });

                    b.OwnsMany("Otp.Core.Domains.ValueObjects.OtpAttempt", "OtpAttempts", b1 =>
                        {
                            b1.Property<Guid>("OtpRequestId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<string>("AttemptStatus")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<DateTime>("AttemptedOn")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Code")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("OtpRequestId", "Id");

                            b1.ToTable("OtpAttempts", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OtpRequestId");
                        });

                    b.OwnsMany("Otp.Core.Domains.ValueObjects.OtpEvent", "Timeline", b1 =>
                        {
                            b1.Property<Guid>("OtpRequestId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<DateTime>("OccuredAt")
                                .HasColumnType("datetime2");

                            b1.Property<string>("Response")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Status")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("OtpRequestId", "Id");

                            b1.ToTable("Timeline", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OtpRequestId");
                        });

                    b.Navigation("App");

                    b.Navigation("ClientInfo");

                    b.Navigation("OtpAttempts");

                    b.Navigation("Timeline");
                });
#pragma warning restore 612, 618
        }
    }
}
