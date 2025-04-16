﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParqueaderoAPI.Data;

#nullable disable

namespace ParqueaderoAPI.Migrations
{
    [DbContext(typeof(ParqueaderoContext))]
    [Migration("20250215184738_UpdateVehiculoModel")]
    partial class UpdateVehiculoModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ParqueaderoAPI.Models.Cliente", b =>
                {
                    b.Property<int>("ClienteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClienteID"));

                    b.Property<string>("Cedula")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClienteID");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ParqueaderoAPI.Models.Espacio", b =>
                {
                    b.Property<int>("EspacioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EspacioID"));

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nomenclatura")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Piso")
                        .HasColumnType("int");

                    b.Property<string>("Zona")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EspacioID");

                    b.ToTable("Espacios");
                });

            modelBuilder.Entity("ParqueaderoAPI.Models.Pago", b =>
                {
                    b.Property<int>("PagoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PagoID"));

                    b.Property<DateTime>("FechaPago")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetodoPago")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ReservaID")
                        .HasColumnType("int");

                    b.HasKey("PagoID");

                    b.HasIndex("ReservaID");

                    b.ToTable("Pagos");
                });

            modelBuilder.Entity("ParqueaderoAPI.Models.Reserva", b =>
                {
                    b.Property<int>("ReservaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservaID"));

                    b.Property<DateTime>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaSalida")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Monto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TipoReserva")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehiculoID")
                        .HasColumnType("int");

                    b.HasKey("ReservaID");

                    b.HasIndex("VehiculoID");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("ParqueaderoAPI.Models.Vehiculo", b =>
                {
                    b.Property<int>("VehiculoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehiculoID"));

                    b.Property<int>("ClienteID")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VehiculoID");

                    b.HasIndex("ClienteID");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("ParqueaderoAPI.Models.Pago", b =>
                {
                    b.HasOne("ParqueaderoAPI.Models.Reserva", "Reserva")
                        .WithMany()
                        .HasForeignKey("ReservaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("ParqueaderoAPI.Models.Reserva", b =>
                {
                    b.HasOne("ParqueaderoAPI.Models.Vehiculo", "Vehiculo")
                        .WithMany()
                        .HasForeignKey("VehiculoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("ParqueaderoAPI.Models.Vehiculo", b =>
                {
                    b.HasOne("ParqueaderoAPI.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });
#pragma warning restore 612, 618
        }
    }
}
