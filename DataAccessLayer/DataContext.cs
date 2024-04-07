using DataAccessLayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Device> Devices { get; set; }

        public DbSet<DeviceType> DeviceTypes { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<DeviceReadingType> DeviceReadingTypes { get; set; }

        public DbSet<Threshold> Thresholds { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        //public DbSet<DeviceReading> DeviceReadings { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Login>().ToTable("logins");

            modelBuilder.Entity<DeviceReadingType>().HasIndex(d => d.Name).IsUnique();
            modelBuilder.Entity<Threshold>().HasIndex(t => new { t.DeviceTypeId, t.DeviceReadingTypeId }).IsUnique();
            modelBuilder.Entity<Threshold>()
                .HasOne(t => t.DeviceType)
                .WithMany(d => d.Thresholds)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Threshold>()
                .HasOne(t => t.DeviceReadingType)
                .WithMany(d => d.Thresholds)
                .OnDelete(DeleteBehavior.NoAction);
            /*
            modelBuilder.Entity<DeviceReading>()
                .HasOne(t => t.Device)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<DeviceReading>()
                .HasOne(t => t.DeviceReadingType)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            */
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.IsActive).HasDefaultValue(true);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleType = "Admin" },
                new Role { Id = 2, RoleType = "User" },
                new Role { Id = 3, RoleType = "Operator" },
                new Role { Id = 4, RoleType = "Observer" }
            );
        }

        
    }
}