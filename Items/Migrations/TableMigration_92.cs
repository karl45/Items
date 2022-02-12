using FluentMigrator;
using FluentMigrator.Infrastructure;
using FluentMigrator.SqlServer;
using Items.Models;
using Items.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Items.Migrations
{
    [Migration(92)]
    public class TableMigration_92 : Migration
    {
        public override void Down()
        {
            Delete.Table("Items");
            Delete.Table("Role");
            Delete.Table("Users");
            Delete.Table("Regions");
            Delete.Table("Orders");
        }

        public override void Up()
        {
            Create.Table("Items")
                    .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1,1)
                    .WithColumn("Name").AsString(50).NotNullable()
                    .WithColumn("Price").AsDecimal().NotNullable()
                    
                    ;

            Create.Table("Roles")
                    .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
                    .WithColumn("Name").AsString(50).NotNullable()
                    ;

            Create.Table("Users")
                    .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
                    .WithColumn("Login").AsString(20).NotNullable().Unique()
                    .WithColumn("Password").AsString(200).NotNullable()
                    .WithColumn("RoleId").AsInt32().NotNullable().ForeignKey("Roles", "Id").OnDelete(System.Data.Rule.Cascade)
                    ;

            Create.Table("Regions")
                    .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
                    .WithColumn("CategoryName").AsString(20).NotNullable()
                    .WithColumn("CategoryValue").AsString(200).NotNullable()
                    .WithColumn("ParentId").AsInt32().Nullable();
                    ;
            
            Create.Table("Orders")
                    .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity(1, 1)
                    .WithColumn("RegionId").AsInt32().NotNullable().ForeignKey("Regions", "Id").OnDelete(System.Data.Rule.Cascade)
                    .WithColumn("OrderDate").AsDateTime().NotNullable()
                    .WithColumn("ItemId").AsInt32().NotNullable().ForeignKey("Items", "Id").OnDelete(System.Data.Rule.Cascade)
                    .WithColumn("Amount").AsInt32()
                    ;

            for (int i = 0; i < 20; i++)
                Insert.IntoTable("Items").Row(new 
                {
                    Name = $"Ball-{i}",
                    Price = 200 + i
                });

            Insert.IntoTable("Roles").Row(new
            {
                Name = "Customer",
            });
            Insert.IntoTable("Roles").Row(new
            {
                Name = "Administrator",
            });

            for (int i = 0; i < 20; i++)
                Insert.IntoTable("Users").Row(new 
                {
                    Login = $"user-{i}",
                    Password = BCrypt.Net.BCrypt.HashPassword($"password-{i}"),
                    RoleId = (new Random()).Next(1,3)
                });

            for (int i = 0; i < 6; i++)
            {
                if(i < 3)
                    Insert.IntoTable("Regions").Row(new
                    {
                        CategoryName = "AREA",
                        CategoryValue = $"AREA-{i}",
                        ParentId = (int?)null
                    });
                else
                    Insert.IntoTable("Regions").Row(new
                    {
                        CategoryName = "CITY",
                        CategoryValue = $"CITY-{i-3}",
                        ParentId = i-3
                    });
            }

            Insert.IntoTable("Regions").Row(new
            {
                CategoryName = "DISTRICT",
                CategoryValue = $"DISTRICT-12",
                ParentId = 5
            });

            Insert.IntoTable("Regions").Row(new
            {
                CategoryName = "DISTRICT",
                CategoryValue = $"DISTRICT-11",
                ParentId = 6
            });
            for (int i = 0; i < 20; i++)
                Insert.IntoTable("Orders").Row(new
                {
                    ItemId = i+1,
                    OrderDate = DateTime.Now,
                    RegionId = (new Random()).Next(4, 9),
                    Amount = i+100
                });

        }
    }
}
