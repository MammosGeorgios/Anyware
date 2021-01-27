namespace Anyware.Migrations
{
    using Anyware.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Anyware.Models.AppUsersDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Anyware.Models.AppUsersDbContext context)
        {
           


            context.VatCategories.AddOrUpdate(x => x.VatCategoryID,
                  new VatCategory() { VatCategoryID = 1, VatName = "Coffee", VatPercentage = 13 },
                  new VatCategory() { VatCategoryID = 2, VatName = "Non-alcoholic beverages", VatPercentage = 13 },
                  new VatCategory() { VatCategoryID = 3, VatName = "Water supplies", VatPercentage = 13 },
                  new VatCategory() { VatCategoryID = 4, VatName = "All others", VatPercentage = 24 }
                  );

            context.ProductCategories.AddOrUpdate(x => x.ProductCategoryID,
                new ProductCategory() { ProductCategoryID = 1, CategoryName = "Fruits and Vegetables", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 2, CategoryName = "Dairy & Chilled Products", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 3, CategoryName = "Fresh Meat & Fish", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 4, CategoryName = "Cheese", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 5, CategoryName = "Cold Cuts", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 6, CategoryName = "Delicatessen", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 7, CategoryName = "Breakfast &snacking", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 8, CategoryName = "Coffes", VatCategoryID = 1 },
                new ProductCategory() { ProductCategoryID = 9, CategoryName = "Basic packaged foods", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 10, CategoryName = "Frozen foods", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 11, CategoryName = "Bakery", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 12, CategoryName = "Wines", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 13, CategoryName = "Spirits, soft drinks, waters", VatCategoryID = 2 },
                new ProductCategory() { ProductCategoryID = 14, CategoryName = "Ready Meals", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 15, CategoryName = "Toiletries", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 16, CategoryName = "Cleaning products", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 17, CategoryName = "Pets", VatCategoryID = 4 },
                new ProductCategory() { ProductCategoryID = 18, CategoryName = "All about baby", VatCategoryID = 4 }
                );


            context.ProductUnitOfMeasurements.AddOrUpdate(x => x.ProductUnitOfMeasurementID,
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 1, UnitName = "Kilogram", UnitAbbreviation = "kg" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 2, UnitName = "Gram", UnitAbbreviation = "gr" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 3, UnitName = "Tonne", UnitAbbreviation = "T" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 4, UnitName = "50 Kilograms", UnitAbbreviation = "50kg" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 5, UnitName = "100 Kilograms", UnitAbbreviation = "100kg" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 6, UnitName = "kilolitre - 1000 L", UnitAbbreviation = "kL" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 7, UnitName = "hectolitre - 100 L", UnitAbbreviation = "hL" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 8, UnitName = "decalitre - 10 L", UnitAbbreviation = "daL" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 9, UnitName = "Litre", UnitAbbreviation = "lt" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 10, UnitName = "millilitre", UnitAbbreviation = "ml" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 11, UnitName = "Piece", UnitAbbreviation = "pc" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 12, UnitName = "Half-dozen", UnitAbbreviation = "hdz" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 13, UnitName = "Dozen", UnitAbbreviation = "dz" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 14, UnitName = "1 Pallet - 24 Cases", UnitAbbreviation = "Pallet" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 15, UnitName = "1 Case - 12 Interpacks ", UnitAbbreviation = "Case" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 16, UnitName = "1 Interpack - 3 Boxes", UnitAbbreviation = "Interpack" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 17, UnitName = "1 Box - 6 Eaches", UnitAbbreviation = "Box" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 18, UnitName = "Bottle", UnitAbbreviation = "Bottle" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 19, UnitName = "1 Carton - 12 Bottles", UnitAbbreviation = "Carton" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 20, UnitName = "1 Bag - 50 Kilograms ", UnitAbbreviation = "Bag" },
                new ProductUnitOfMeasurement() { ProductUnitOfMeasurementID = 21, UnitName = "100 Pieces", UnitAbbreviation = "100pc" }
              );


            context.Products.AddOrUpdate(x => x.ProductID,
                new Product()
                {
                    ProductID = 1,
                    ProductName = "Eggs",
                    Description = "A half dozen of eggs",
                    ProductStatus = ProductStatus.InSupply,
                    ProductCategoryID = 2,
                    ProductPrice = 18,
                    ProductUnitOfMeasurementID = 17,
                    UnitsInStock = 1000
                },

                new Product()
                {
                    ProductID = 2,
                    ProductName = "Milk NOYNOY Family ",
                    Description = "Fresh Milk",
                    ProductStatus = ProductStatus.InSupply,
                    ProductCategoryID = 2,
                    ProductPrice = 200,
                    ProductUnitOfMeasurementID = 15,
                    UnitsInStock = 1000
                },

                new Product()
                {
                    ProductID = 3,
                    ProductName = "Tomatoes",
                    Description = "Fresh tomatoes",
                    ProductStatus = ProductStatus.OutOfSupply,
                    ProductCategoryID = 1,
                    ProductPrice = 45,
                    ProductUnitOfMeasurementID = 20,
                    UnitsInStock = 0
                },

                 new Product()
                 {
                     ProductID = 4,
                     ProductName = "Nuggets",
                     Description = "Fresh chciken nuggets",
                     ProductStatus = ProductStatus.OutOfSupply,
                     ProductCategoryID = 3,
                     ProductPrice = 300,
                     ProductUnitOfMeasurementID = 14,
                     UnitsInStock = 0
                 },
                 new Product()
                 {
                     ProductID = 5,
                     ProductName = "Pampers",
                     Description = "Diaper shorts",
                     ProductStatus = ProductStatus.InSupply,
                     ProductCategoryID = 15,
                     ProductPrice = 2500,
                     ProductUnitOfMeasurementID = 15,
                     UnitsInStock = 1000
                 },
                 new Product()
                 {
                     ProductID = 6,
                     ProductName = "Advance Yogurt",
                     Description = "Yogurt with cereals",
                     ProductStatus = ProductStatus.InSupply,
                     ProductCategoryID = 18,
                     ProductPrice = 30,
                     ProductUnitOfMeasurementID = 16,
                     UnitsInStock = 1000
                 },
                 new Product()
                 {
                     ProductID = 7,
                     ProductName = "Baby wipe",
                     Description = "Baby wipe Aqua Pure",
                     ProductStatus = ProductStatus.InSupply,
                     ProductCategoryID = 18,
                     ProductPrice = 70,
                     ProductUnitOfMeasurementID = 16,
                     UnitsInStock = 1000
                 },
                    new Product()
                    {
                        ProductID = 8,
                        ProductName = "Sea Bream",
                        Description = "Fresh Fish",
                        ProductStatus = ProductStatus.InSupply,
                        ProductCategoryID = 3,
                        ProductPrice = 45,
                        ProductUnitOfMeasurementID = 4,
                        UnitsInStock = 1000
                    },
                    new Product()
                    {
                        ProductID = 9,
                        ProductName = "Salmon",
                        Description = "Fresh Fish",
                        ProductStatus = ProductStatus.OutOfSupply,
                        ProductCategoryID = 3,
                        ProductPrice = 140,
                        ProductUnitOfMeasurementID = 5,
                        UnitsInStock = 0
                    },
                     new Product()
                     {
                         ProductID = 10,
                         ProductName = "Chicken",
                         Description = "Fresh chicken",
                         ProductStatus = ProductStatus.InSupply,
                         ProductCategoryID = 3,
                         ProductPrice = 200,
                         ProductUnitOfMeasurementID = 4,
                         UnitsInStock = 1000
                     },

                        new Product()
                        {
                            ProductID = 11,
                            ProductName = "Banana",
                            Description = "Fresh bananas",
                            ProductStatus = ProductStatus.InSupply,
                            ProductCategoryID = 1,
                            ProductPrice = 100,
                            ProductUnitOfMeasurementID = 5
                        },
                        new Product()
                        {
                            ProductID = 12,
                            ProductName = "Muesli",
                            Description = "Cereals",
                            ProductStatus = ProductStatus.InSupply,
                            ProductCategoryID = 7,
                            ProductPrice = 400,
                            ProductUnitOfMeasurementID = 15
                        },

                         new Product()
                         {
                             ProductID = 13,
                             ProductName = "Espresso",
                             Description = "Coffee",
                             ProductStatus = ProductStatus.InSupply,
                             ProductCategoryID = 8,
                             ProductPrice = 1000,
                             ProductUnitOfMeasurementID = 16
                         },

                         new Product()
                         {
                             ProductID = 14,
                             ProductName = "Nescafe",
                             Description = "Coffee",
                             ProductStatus = ProductStatus.OutOfSupply,
                             ProductCategoryID = 8,
                             ProductPrice = 800,
                             ProductUnitOfMeasurementID = 16,
                             UnitsInStock = 0
                         },
                         new Product()
                         {
                             ProductID = 15,
                             ProductName = "Coca Cola",
                             Description = "Coca Cola",
                             ProductStatus = ProductStatus.InSupply,
                             ProductCategoryID = 13,
                             ProductPrice = 650,
                             ProductUnitOfMeasurementID = 16,
                             UnitsInStock = 1000
                         },
                         new Product()
                         {
                             ProductID = 16,
                             ProductName = "IOLI",
                             Description = "Water",
                             ProductStatus = ProductStatus.InSupply,
                             ProductCategoryID = 13,
                             ProductPrice = 200,
                             ProductUnitOfMeasurementID = 16,
                             UnitsInStock = 2000
                         },
                          new Product()
                          {
                              ProductID = 17,
                              ProductName = "Olive Oil",
                              Description = "Extra Vergin Olive Oil",
                              ProductStatus = ProductStatus.InSupply,
                              ProductCategoryID = 9,
                              ProductPrice = 70,
                              ProductUnitOfMeasurementID = 19,
                              UnitsInStock = 1000
                          },
                           new Product()
                           {
                               ProductID = 18,
                               ProductName = "Spaghetti",
                               Description = "Spaghetti 500gr",
                               ProductStatus = ProductStatus.InSupply,
                               ProductCategoryID = 9,
                               ProductPrice = 2600,
                               ProductUnitOfMeasurementID = 14,
                               UnitsInStock = 2000
                           },
                           new Product()
                           {
                               ProductID = 19,
                               ProductName = "Lettuce",
                               Description = "Fresh Lettuce",
                               ProductStatus = ProductStatus.InSupply,
                               ProductCategoryID = 1,
                               ProductPrice = 250,
                               ProductUnitOfMeasurementID = 21,
                               UnitsInStock = 2000
                           },
                           new Product()
                           {
                               ProductID = 20,
                               ProductName = "Potatoes",
                               Description = "Fresh Potatoes",
                               ProductStatus = ProductStatus.InSupply,
                               ProductCategoryID = 1,
                               ProductPrice = 300,
                               ProductUnitOfMeasurementID = 20,
                               UnitsInStock = 2000
                           }
              );
            context.Vendors.AddOrUpdate(x => x.VendorID, new Vendor()
            {
                VendorID = 1,
                VendorName = "AnyWare",
                VendorAFM = "000000000",
                VendorLegalName = "AnyWare S.A.",
                VendorDOI = "ATHENS"
            },
            new Vendor()
            {
                VendorID = 2,
                VendorName = "N/A",
                VendorAFM = "000000000",
                VendorLegalName = "Unvalidated Users",
                VendorDOI = "-"
            }
            );


            context.Users.AddOrUpdate(x => x.Id, new ApplicationUser()
            {
                Id = "0f429f0a-62af-4995-acb2-afe5d5b5f345",
                Email = "FirstValidatedUser@usernroles.com",
                EmailConfirmed = false,
                PasswordHash = "AI3JK0lRdQlBnN6cbZUtDgDktEaK6DI/Ck2hVGSD5rszgweduKNi2Mb/QVMrFzLAUQ==",
                SecurityStamp = "dbf30e88-f17d-488b-8ca8-47aece4ffa03",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                UserName = "FirstValidatedUser@usernroles.com",
                FirstName = "AUTOGENERATED",
                MiddleName = "A",
                LastName = "VERIFIED",
                DateofBirth = DateTime.Now,
                PersonalPhone = "0000000000",
                VendorID = 1
            },
                new ApplicationUser()
                {
                    Id = "bfd25793-f62a-46ec-9ea2-832d62cb9c97",
                    Email = "FirstUnvalidatedUser@usernroles.com",
                    EmailConfirmed = false,
                    PasswordHash = "AOMaX0CVgAW5LJgRgXwneMIiDDAqfEcAzjFlx2GhyOX2jdTvYTFPr/STeX9uX9mKug==",
                    SecurityStamp = "1eafd31a-c09c-402c-be40-c2168895f686",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "FirstUnvalidatedUser@usernroles.com",
                    FirstName = "AUTOGENERATED",
                    MiddleName = "A",
                    LastName = "UNVERIFIED",
                    DateofBirth = DateTime.Now,
                    PersonalPhone = "0000000000",
                    VendorID = 1
                },
                new ApplicationUser()
                {
                    Id = "9e7525ce-9426-450f-83cb-f53cb49fabd4",
                    Email = "UserNRoles@usernroles.com",
                    EmailConfirmed = false,
                    PasswordHash = "AEtjksZmeAhZ86pax6te7H3uXlWD6gCVfcOnaovpBySaNv/Nw9k+LCdMKDuiBlPLmA==",
                    SecurityStamp = "72276c48-c692-48dc-8f4e-0b63d1cf8287",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "UserNRoles@usernroles.com",
                    FirstName = "AUTOGENERATED",
                    MiddleName = "A",
                    LastName = "ADMIN",
                    DateofBirth = DateTime.Now,
                    PersonalPhone = "0000000000",
                    VendorID = 1
                },
                new ApplicationUser()
                {
                    Id = "d76c9d60-34f4-48c5-9c49-6559a4822346",
                    Email = "ApplicationManager@usernroles.com",
                    EmailConfirmed = false,
                    PasswordHash = "AGvUhkYc9FjSQcCq6U31P3AsQMXUjdzylZeduASH5L6Fk1s4r46ISqWw+Va/O0yR/Q==",
                    SecurityStamp = "cc8c0820-0bf1-4b63-b551-408bda21b88c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "ApplicationManager@usernroles.com",
                    FirstName = "AUTOGENERATED",
                    MiddleName = "A",
                    LastName = "MANAGER",
                    DateofBirth = DateTime.Now,
                    PersonalPhone = "0000000000",
                    VendorID = 1
                }
            );




        }
    }
}
