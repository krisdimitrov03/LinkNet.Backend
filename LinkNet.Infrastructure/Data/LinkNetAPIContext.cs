using LinkNet.Infrastructure.Data.Models;
using LinkNet.Infrastructure.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkNet.Infrastructure.Data;

public class LinkNetAPIContext : IdentityDbContext<ApplicationUser>
{
    public LinkNetAPIContext(DbContextOptions<LinkNetAPIContext> options)
        : base(options)
    {
    }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Occupation> Occupations { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Gender> Genders { get; set; }

    public DbSet<TokenLog> TokenLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
