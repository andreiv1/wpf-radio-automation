using Microsoft.EntityFrameworkCore;
using RA.Database.Models;
using RA.Database.Config;
using RA.Database.Models.Abstract;

namespace RA.Database
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistTrack> ArtistsTracks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryHierarchy> CategoriesHierarchy { get; set; }
        public DbSet<Clock> Clocks { get; set; }
        public DbSet<ClockItemBase> ClockItems { get; set; }
        public DbSet<ClockItemCategory> ClockItemsCategories { get; set; }
        public DbSet<ClockItemCategoryTag> ClockItemCategoryTags { get; set; }
        public DbSet<ClockItemEvent> ClockItemsEvents { get; set; }
        public DbSet<ClockItemTrack> ClockItemsTracks { get; set; }
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
        public DbSet<UserGroupRule> UserRules { get; set; }
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
            modelBuilder.ApplyConfiguration(new TrackTagConfig());
            modelBuilder.ApplyConfiguration(new PlaylistConfig());
            modelBuilder.ApplyConfiguration(new ClockItemConfig());
            modelBuilder.ApplyConfiguration(new ClockItemCategoryTagConfig());
            modelBuilder.Entity<CategoryHierarchy>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("CategoriesHierarchy");
            });

            modelBuilder.Entity<Category>().HasData(
                new Category(){Id = 1, Name = "Music",Color = "#292928"},
                new Category(){Id = 2, Name = "Station ID",Color = "#f91212"},
                new Category(){Id = 3, Name = "Commercials",Color = "#0ac720"},
                new Category(){Id = 4, Name = "Shows",Color = "#d016f5"},
                new Category(){Id = 5, Name = "News",Color="#001dd9"}
            );
            modelBuilder.Entity<TagCategory>().HasData(
                new TagCategory { Id = 1, Name = "Genre", IsBuiltIn = true },
                new TagCategory { Id = 2, Name = "Language", IsBuiltIn = true },
                new TagCategory { Id = 3, Name = "Mood", IsBuiltIn = true }
            );

            modelBuilder.Entity<TagValue>().HasData(
                new TagValue { Id = 1, TagCategoryId = 2, Name = "English"},
                new TagValue { Id = 2, TagCategoryId = 2, Name = "French"},
                new TagValue { Id = 3, TagCategoryId = 2, Name = "Romanian"}
            );

            modelBuilder.Entity<TagValue>().HasData(
                new TagValue { Id = 4, TagCategoryId = 3, Name = "Happy"},
                new TagValue { Id = 5, TagCategoryId = 3, Name = "Sad"},
                new TagValue { Id = 6, TagCategoryId = 3, Name = "Energetic"}
            );

            modelBuilder.Entity<TagValue>().HasData(
                new TagValue { Id = 7, TagCategoryId = 1, Name = "Rock"},
                new TagValue { Id = 8, TagCategoryId = 1, Name = "Pop"},
                new TagValue { Id = 9, TagCategoryId = 1, Name = "Dance"}
            );

            modelBuilder.Entity<UserGroup>().HasData(
                new UserGroup { Id = 1, Name = "Administrator"}
            );

            modelBuilder.Entity<UserGroupRule>().HasData(new UserGroupRule[]
            {
                new UserGroupRule { Id = 1, RuleValue = UserRuleType.ACCESS_MEDIA_LIBRARY, UserGroupId = 1 },
                new UserGroupRule { Id = 2, RuleValue = UserRuleType.ACCESS_PLANNER, UserGroupId = 1 },
                new UserGroupRule { Id = 3, RuleValue = UserRuleType.ACCESS_REPORTS, UserGroupId = 1 },
                new UserGroupRule { Id = 4, RuleValue = UserRuleType.ACCESS_SETTINGS, UserGroupId = 1 },
            });

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                FullName = "Andrei",
                Username = "andrei",
                Password = "$2a$11$SWq88W6Q77w7sanz7HrxbexnTN0nLq8XB70lLFrSDQbddPzmnQdIK",
                UserGroupId = 1
            });

        }
    }
}
