using Microsoft.EntityFrameworkCore;
using Snaptwit.Shared;

namespace Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Followers> Followers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Followers>()
                    .HasOne(m => m.FollowedUser)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(m => m.FollowedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Followers>()
                    .HasOne(m => m.FollowingUser)
                    .WithMany(u => u.Following)
                    .HasForeignKey(m => m.FollowingUserId)
                    .OnDelete(DeleteBehavior.Restrict);
    }
}