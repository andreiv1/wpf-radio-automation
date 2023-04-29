using Microsoft.EntityFrameworkCore;
using RA.Database.Models;
using RA.Database.Config;


namespace RA.Database
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistTrack> ArtistsTracks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryHierarchy> CategoriesHierarchy { get; set; }
        public DbSet<Clock> Clocks { get; set; }
        public DbSet<ClockItem> ClockItems { get; set; }
        public DbSet<ClockTemplate> ClockTemplates { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistItem> PlaylistItems { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<DefaultSchedule> DefaultSchedules { get; set; }
        public DbSet<PlannedSchedule> PlannedSchedules { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TagCategory> TagCategories { get; set; }
        public DbSet<TagValue> TagValues { get; set; }
        public DbSet<TrackHistory> TrackHistory { get; set; }
        public DbSet<TrackTag> TrackTags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserRule> UserRules { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // This constructor is typically used by the Entity Framework Core infrastructure
            // to create an instance of your DbContext with the options that it has configured.
        }

        public AppDbContext()
        {
            throw new NotImplementedException();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            //    .EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ArtistTrackConfig());
            modelBuilder.ApplyConfiguration(new TrackConfig());
            modelBuilder.ApplyConfiguration(new ClockTemplateConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new UserGroupConfig());
            modelBuilder.ApplyConfiguration(new ScheduleConfig());
            modelBuilder.ApplyConfiguration(new UserRuleConfig());
            modelBuilder.ApplyConfiguration(new UserGroupConfig());
            modelBuilder.ApplyConfiguration(new TrackHistoryConfig());
            modelBuilder.ApplyConfiguration(new TrackTagConfig());
            modelBuilder.ApplyConfiguration(new PlaylistConfig());

            modelBuilder.Entity<CategoryHierarchy>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("CategoriesHierarchy");
            });
        }

    }
}
