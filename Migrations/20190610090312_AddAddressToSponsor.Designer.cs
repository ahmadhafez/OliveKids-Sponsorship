﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OliveKids.Repository;

namespace OliveKids.Migrations
{
    [DbContext(typeof(OkSposershipContext))]
    [Migration("20190610090312_AddAddressToSponsor")]
    partial class AddAddressToSponsor
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OliveKids.Models.Kid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArabicName");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Description")
                        .HasMaxLength(2000);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("SponsorId");

                    b.HasKey("Id");

                    b.HasIndex("SponsorId");

                    b.ToTable("Kid");
                });

            modelBuilder.Entity("OliveKids.Models.Sponsor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("CommunicationPrefrence");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Language");

                    b.Property<string>("Mobile")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Notes")
                        .HasMaxLength(4000);

                    b.Property<string>("PaymentMethod");

                    b.Property<string>("Receipt");

                    b.HasKey("Id");

                    b.ToTable("Sponsor");
                });

            modelBuilder.Entity("OliveKids.Models.Kid", b =>
                {
                    b.HasOne("OliveKids.Models.Sponsor")
                        .WithMany("SponsoredKids")
                        .HasForeignKey("SponsorId");
                });
#pragma warning restore 612, 618
        }
    }
}
