﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORMtest.Entities
{
    public class TrainingDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;


        public TrainingDbContext(string connectionString)
        {
            _connectionString = @"Server=BS-1027\\SQLEXPRESS2;Database=courseDb;Trusted_Connection=True;\r\n";
            _migrationAssembly = Assembly.GetExecutingAssembly().GetName().Name;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(""));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
/*            modelBuilder.Entity<Topic>().HasKey((x) => new {x.topicID,x.});

            // key of courseRegi table
            modelBuilder.Entity<AssignedCourse>().HasKey((x) => new { x.CourseID, x.TeacherID });
            modelBuilder.Entity<Attendance>().HasKey((x) => new { x.CourseID, x.StudentID, x.date });
        */    
            
            base.OnModelCreating(modelBuilder);
        }



        DbSet<Course> courses { get; set; }


    }
}
