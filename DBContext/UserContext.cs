using MessangerServer.Database_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MessangerServer.DBContext
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BanList> BanList { get; set; }
        public UserContext() : base(new DbContextOptions<UserContext>())
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            this.Users.AddRange(new List<User>()
            {
                new User() { Id = 1, Name = "User1", Login="Login1", Password="Password1" },
                new User() { Id = 2, Name = "User2", Login="Login2", Password="Password2" }
            });
            this.SaveChanges();
        }

        public bool IsUserBanned(User user)
        {
            if (this.BanList.FirstOrDefault(b => b.User_ID==user.Id)!=null)
                return true;
            return false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=UserDatabase.db");
        }
    }
}
