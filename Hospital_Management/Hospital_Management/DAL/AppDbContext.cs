using Hospital_Management.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace Hospital_Management.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IHttpContextAccessor _http;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor http) : base(options)
        {
            _http = http;
        }
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<MedicalCard> MedicalCards { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<BaseEntity>();
            string? name = _http?.HttpContext?.User?.Identity?.IsAuthenticated == true
                           ? _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : "System";

            foreach (var data in entities)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreateAt = DateTime.Now;
                        if (!string.IsNullOrEmpty(name)) data.Entity.CreatedById = name;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdateAt = DateTime.Now;
                        if (!string.IsNullOrEmpty(name)) data.Entity.CreatedById = name;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
