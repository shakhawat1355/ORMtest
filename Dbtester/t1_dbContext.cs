using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dbtester
{
    public class t1_dbContext : DbContext
    {
        private readonly string _ConnectionString;
        private readonly string _migrationAssembly;

        public t1_dbContext()
        {
            _ConnectionString = @"Server=BS-1027\SQLEXPRESS;Database=final;Trusted_Connection = True;";
            //  Server = BS - 1129\\SQLEXPRESS; Database = MVCdb; Trusted_Connection = True; Encrypt = False
            //"Server = BS-1027//SQLEXPRESS; Database = final; Trusted_Connection = True; Encrypt = False";
            //@"Server=DESKTOP-8VMMQPN\SQLEXPRESS;Database=final;Trusted_Connection = True;";
            _migrationAssembly = Assembly.GetExecutingAssembly().GetName().Name;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_ConnectionString, (x) =>
                {
                    x.MigrationsAssembly(_migrationAssembly);

                });
            }
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Course>().ToTable("AssignedCourses2");
            //modelBuilder.Entity<CourseRegistration>().ToTable("CourseRegistrations");


            //modelBuilder.Entity<Course>()
            //.HasOne<Teacher>(s => s.Teacher)
            //.WithMany(g => g.AssignedCourses)
            //.HasForeignKey(s => s.TeacherID);





            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Test1> test1 { get; set; }

    }
}
