using Appointment.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.DataAccess.Context
{
    public class AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : DbContext(options)
    {
        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
