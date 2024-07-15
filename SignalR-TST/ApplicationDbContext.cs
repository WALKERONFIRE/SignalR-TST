using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR_TST.Models;

namespace SignalR_TST
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<FileMessage> FileMessages { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
    }
}
