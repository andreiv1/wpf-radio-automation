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
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistItem> PlaylistItems { get; set; }
        public DbSet<ScheduleDefault> SchedulesDefault { get; set; }
        public DbSet<ScheduleDefaultItem> ScheduleDefaultItems { get; set; }
        public DbSet<SchedulePlanned> SchedulesPlanned { get; set; }
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

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ArtistTrackConfig());
            modelBuilder.ApplyConfiguration(new TrackConfig());
            modelBuilder.ApplyConfiguration(new ClockTemplateConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new UserGroupConfig());
            modelBuilder.ApplyConfiguration(new UserRuleConfig());
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
