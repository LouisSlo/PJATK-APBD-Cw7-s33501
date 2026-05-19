using Czwiczenie7.Modele;
using Microsoft.EntityFrameworkCore;

namespace Czwiczenie7.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<PC> PCs { get; set; }
    public DbSet<PCComponent> PCComponents { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PC>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).HasMaxLength(50).IsRequired();

        });

        modelBuilder.Entity<ComponentType>(e =>
        {
            e.HasKey(ct => ct.Id);
            e.Property(ct => ct.Abbreviation).HasMaxLength(30).IsRequired();
            e.Property(ct => ct.Name).HasMaxLength(150).IsRequired();

        });

        modelBuilder.Entity<ComponentManufacturer>(e =>
        {
            e.HasKey(cm => cm.Id);
            e.Property(cm => cm.Abbreviation).HasMaxLength(30).IsRequired();
            e.Property(cm => cm.FullName).HasMaxLength(300).IsRequired();
            e.Property(cm => cm.FoundationDate).HasColumnType("date");
        });

        modelBuilder.Entity<Component>(e =>
        { 
            e.HasKey(c => c.Code);
            e.Property(c => c.Code).HasMaxLength(10).IsFixedLength().IsRequired();
            e.Property(c => c.Name).HasMaxLength(300).IsRequired();
            
            e.HasOne(c => c.Manufacturer)
                .WithMany(m => m.Components)
                .HasForeignKey(c => c.ComponentManufacturerId);

            e.HasOne(c => c.Type)
                .WithMany(t => t.Components)
                .HasForeignKey(c => c.ComponentTypeId);

        });
        
        modelBuilder.Entity<PCComponent>(e =>
        {
            e.HasKey(pcc => new { pcc.PCId, pcc.ComponentCode });
            e.Property(pcc => pcc.ComponentCode).HasMaxLength(10).IsFixedLength();
            
            e.HasOne(pcc => pcc.PC)
                .WithMany(pc => pc.PCComponents)
                .HasForeignKey(pcc => pcc.PCId);

            e.HasOne(pcc => pcc.Component)
                .WithMany(c => c.PCComponents)
                .HasForeignKey(pcc => pcc.ComponentCode);
        });

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateTime(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateTime(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Gaming Inc.", FoundationDate = new DateTime(1994, 1, 1) }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "CPU0000001", Name = "Ryzen 7 7800X3D", Description = "8-core gaming processor", ComponentManufacturerId = 1, ComponentTypeId = 1 },
            new Component { Code = "GPU0000001", Name = "RTX 4080 Super", Description = "High-end gaming graphics card", ComponentManufacturerId = 2, ComponentTypeId = 2 },
            new Component { Code = "RAM0000001", Name = "Corsair Vengeance DDR5 16GB", Description = "DDR5 RAM module 16GB", ComponentManufacturerId = 3, ComponentTypeId = 3 }
        );

        modelBuilder.Entity<PC>().HasData(
            new PC { Id = 1, Name = "Gaming Beast X", Weight = 12.5f, Warranty = 36, CreatedAt = DateTime.Parse("2026-05-08T09:00:00"), Stock = 5 },
            new PC { Id = 2, Name = "Office Mini Pro", Weight = 4.2f, Warranty = 24, CreatedAt = DateTime.Parse("2026-04-15T13:30:00"), Stock = 12 },
            new PC { Id = 3, Name = "Home Media Center", Weight = 6.0f, Warranty = 24, CreatedAt = DateTime.Parse("2026-05-10T10:00:00"), Stock = 2 }
        );

        modelBuilder.Entity<PCComponent>().HasData(
            new PCComponent { PCId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PCComponent { PCId = 1, ComponentCode = "RAM0000001", Amount = 2 },
            new PCComponent { PCId = 2, ComponentCode = "CPU0000001", Amount = 1 },
            new PCComponent { PCId = 3, ComponentCode = "RAM0000001", Amount = 1 }
        );
    }
}













