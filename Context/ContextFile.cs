using Microsoft.EntityFrameworkCore;
using ServerApi.Models;

namespace ServerApi.Context
{
    public class ContextFile:DbContext
    {
        public ContextFile(DbContextOptions opt) : base(opt) { }

        public DbSet<DataDbModel> Data { get; set; }
        public DbSet<UserDbModel> User { get; set; }
        public DbSet<NotificationDbModel> Notification { get; set; }
        public DbSet<ServerApi.Models.HideDbModel> HideDbModel { get; set; }
        public DbSet<ChatDbModel> ChatDbModel { get; set; }
        public DbSet<ChatForWho> chatForWho { get; set; }
    }
}
