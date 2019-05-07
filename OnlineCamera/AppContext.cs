using Microsoft.EntityFrameworkCore;
using OnlineCamera.Entety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCamera
{
    public class AppContext : DbContext
    {
        public DbSet<CameraStatus> Posts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=online_camera.db");
        }
    }
}
