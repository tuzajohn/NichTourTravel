using Microsoft.EntityFrameworkCore;
using Niche.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Niche.Core.Contexts
{
    public class NicheContext : DbContext
    {
        public NicheContext(DbContextOptions<NicheContext> options) : base(options) { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
