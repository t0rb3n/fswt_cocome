﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TradingSystem.inventory.data;

#nullable disable

namespace TradingSystem.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220126211830_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TradingSystem.inventory.data.enterprise.Enterprise", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_enterprises");

                    b.ToTable("enterprises", (string)null);
                });

            modelBuilder.Entity("TradingSystem.inventory.data.enterprise.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("Barcode")
                        .HasColumnType("bigint")
                        .HasColumnName("barcode");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("ProductSupplierId")
                        .HasColumnType("bigint")
                        .HasColumnName("product_supplier_id");

                    b.Property<double>("PurchasePrice")
                        .HasColumnType("double precision")
                        .HasColumnName("purchase_price");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("ProductSupplierId")
                        .HasDatabaseName("ix_products_product_supplier_id");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("TradingSystem.inventory.data.enterprise.ProductSupplier", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("EnterpriseId")
                        .HasColumnType("bigint")
                        .HasColumnName("enterprise_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_product_suppliers");

                    b.HasIndex("EnterpriseId")
                        .HasDatabaseName("ix_product_suppliers_enterprise_id");

                    b.ToTable("product_suppliers", (string)null);
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.OrderEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasColumnName("product_id");

                    b.Property<long>("ProductOrderId")
                        .HasColumnType("bigint")
                        .HasColumnName("product_order_id");

                    b.HasKey("Id")
                        .HasName("pk_order_entries");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_order_entries_product_id");

                    b.HasIndex("ProductOrderId")
                        .HasDatabaseName("ix_order_entries_product_order_id");

                    b.ToTable("order_entries", (string)null);
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.ProductOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("delivery_date");

                    b.Property<DateTime>("OrderingDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ordering_date");

                    b.Property<long>("StoreId")
                        .HasColumnType("bigint")
                        .HasColumnName("store_id");

                    b.HasKey("Id")
                        .HasName("pk_product_orders");

                    b.HasIndex("StoreId")
                        .HasDatabaseName("ix_product_orders_store_id");

                    b.ToTable("product_orders", (string)null);
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.StockItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.Property<int>("MaxStock")
                        .HasColumnType("integer")
                        .HasColumnName("max_stock");

                    b.Property<int>("MinStock")
                        .HasColumnType("integer")
                        .HasColumnName("min_stock");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasColumnName("product_id");

                    b.Property<double>("SalesPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("sales_price");

                    b.Property<long>("StoreId")
                        .HasColumnType("bigint")
                        .HasColumnName("store_id");

                    b.HasKey("Id")
                        .HasName("pk_stock_items");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_stock_items_product_id");

                    b.HasIndex("StoreId")
                        .HasDatabaseName("ix_stock_items_store_id");

                    b.ToTable("stock_items", (string)null);
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.Store", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("EnterpriseId")
                        .HasColumnType("bigint")
                        .HasColumnName("enterprise_id");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("location");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_stores");

                    b.HasIndex("EnterpriseId")
                        .HasDatabaseName("ix_stores_enterprise_id");

                    b.ToTable("stores", (string)null);
                });

            modelBuilder.Entity("TradingSystem.inventory.data.enterprise.Product", b =>
                {
                    b.HasOne("TradingSystem.inventory.data.enterprise.ProductSupplier", "ProductSupplier")
                        .WithMany("Products")
                        .HasForeignKey("ProductSupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_product_suppliers_product_supplier_id");

                    b.Navigation("ProductSupplier");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.enterprise.ProductSupplier", b =>
                {
                    b.HasOne("TradingSystem.inventory.data.enterprise.Enterprise", null)
                        .WithMany("ProductSuppliers")
                        .HasForeignKey("EnterpriseId")
                        .HasConstraintName("fk_product_suppliers_enterprises_enterprise_id");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.OrderEntry", b =>
                {
                    b.HasOne("TradingSystem.inventory.data.enterprise.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_entries_products_product_id");

                    b.HasOne("TradingSystem.inventory.data.store.ProductOrder", "ProductOrder")
                        .WithMany("OrderEntries")
                        .HasForeignKey("ProductOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_entries_product_orders_product_order_id");

                    b.Navigation("Product");

                    b.Navigation("ProductOrder");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.ProductOrder", b =>
                {
                    b.HasOne("TradingSystem.inventory.data.store.Store", "Store")
                        .WithMany("ProductOrders")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_orders_stores_store_id");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.StockItem", b =>
                {
                    b.HasOne("TradingSystem.inventory.data.enterprise.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stock_items_products_product_id");

                    b.HasOne("TradingSystem.inventory.data.store.Store", "Store")
                        .WithMany("StockItems")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stock_items_stores_store_id");

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.Store", b =>
                {
                    b.HasOne("TradingSystem.inventory.data.enterprise.Enterprise", "Enterprise")
                        .WithMany("Stores")
                        .HasForeignKey("EnterpriseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stores_enterprises_enterprise_id");

                    b.Navigation("Enterprise");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.enterprise.Enterprise", b =>
                {
                    b.Navigation("ProductSuppliers");

                    b.Navigation("Stores");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.enterprise.ProductSupplier", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.ProductOrder", b =>
                {
                    b.Navigation("OrderEntries");
                });

            modelBuilder.Entity("TradingSystem.inventory.data.store.Store", b =>
                {
                    b.Navigation("ProductOrders");

                    b.Navigation("StockItems");
                });
#pragma warning restore 612, 618
        }
    }
}
