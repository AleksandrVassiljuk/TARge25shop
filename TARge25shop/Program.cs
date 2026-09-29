using Microsoft.EntityFrameworkCore;
using TARge25shop.ApplicationServices.Services;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Data;

namespace TARge25Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            // Database
            builder.Services.AddDbContext<TARge25ShopContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Services
            builder.Services.AddScoped<ISpaceshipServices, SpaceshipServices>();
            builder.Services.AddScoped<IKindergartenServices, KindergartenServices>();
            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();

            var app = builder.Build();

            // Configure HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Vajalik wwwroot failide ja üleslaetud piltide jaoks
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            // ==========================================
            // DATABASE SETUP
            // ==========================================
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<TARge25ShopContext>();

                dbContext.Database.EnsureCreated();

                // ==========================================
                // KINDERGARTEN TABLE
                // ==========================================
                dbContext.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[Kindergartens]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Kindergartens]
    (
        [Id] uniqueidentifier NOT NULL PRIMARY KEY,
        [GroupName] nvarchar(200) NOT NULL,
        [ChildrenCount] int NOT NULL,
        [KindergartenName] nvarchar(200) NOT NULL,
        [TeacherName] nvarchar(200) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL
    );
END
");

                // ==========================================
                // REAL ESTATE TABLE
                // ==========================================
                dbContext.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[RealEstates]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[RealEstates]
    (
        [Id] uniqueidentifier NOT NULL PRIMARY KEY,
        [Address] nvarchar(200) NOT NULL,
        [PropertyType] nvarchar(100) NOT NULL,
        [Rooms] int NOT NULL,
        [Area] float NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL
    );
END
");

                // ==========================================
                // REAL ESTATE IMAGES TABLE
                // ==========================================
                dbContext.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[RealEstateImages]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[RealEstateImages]
    (
        [Id] uniqueidentifier NOT NULL PRIMARY KEY,
        [FilePath] nvarchar(max) NOT NULL,
        [RealEstateId] uniqueidentifier NOT NULL,

        CONSTRAINT [FK_RealEstateImages_RealEstates_RealEstateId]
        FOREIGN KEY ([RealEstateId])
        REFERENCES [dbo].[RealEstates] ([Id])
        ON DELETE CASCADE
    );

    CREATE INDEX [IX_RealEstateImages_RealEstateId]
    ON [dbo].[RealEstateImages] ([RealEstateId]);
END
");
            }

            app.Run();
        }
    }
}