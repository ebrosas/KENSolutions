using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KenHRApp.Domain.Entities;
using System.Reflection.Metadata;
using KenHRApp.Infrastructure.EntityConfiguration;

namespace KenHRApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        #region Map SQL Tables to Entities
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<IdentityProof> IdentityProofs => Set<IdentityProof>();
        public DbSet<Qualification> Qualifications => Set<Qualification>();
        public DbSet<EmployeeSkill> EmployeeSkills => Set<EmployeeSkill>();
        public DbSet<EmployeeCertification> EmployeeCertifications => Set<EmployeeCertification>();
        public DbSet<LanguageSkill> LanguageSkills => Set<LanguageSkill>();
        public DbSet<FamilyMember> FamilyMembers => Set<FamilyMember>();
        public DbSet<FamilyVisa> FamilyVisas => Set<FamilyVisa>();
        public DbSet<EmploymentHistory> EmploymentHistories => Set<EmploymentHistory>();
        public DbSet<OtherDocument> OtherDocuments => Set<OtherDocument>();
        public DbSet<EmployeeTransaction> EmployeeTransactions => Set<EmployeeTransaction>();
        #endregion

        #region Initialize entities    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the default schema that will apply to all entities
            modelBuilder.HasDefaultSchema("kenuser");

            // Configure SQL Server to be case-insensitive
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            //new EmployeeEntityConfig().Configure(modelBuilder.Entity<Employee>());

            #region Configure models
            modelBuilder.Entity<IdentityProof>(
               entity =>
               {
                   entity.ToTable("IdentityProof");
                   entity.HasKey(c => c.AutoId)
                       .HasName("PK_IdentityProof_AutoId");

                   entity.HasIndex(e => new { e.PassportNumber, e.DateOfIssue, e.DateOfExpiry })
                       .HasDatabaseName("IX_IdentityProof_PassportInfo")
                       .IsUnique();

                   entity.HasIndex(e => new { e.ContractNumber, e.ContractDateOfIssue, e.ContractDateOfExpiry })
                       .HasDatabaseName("IX_IdentityProof_ContractInfo");

                   entity.HasIndex(e => new { e.VisaNumber, e.VisaTypeCode, e.Profession })
                       .HasDatabaseName("IX_IdentityProof_ContractInfo")
                       .IsUnique()
                       .HasFilter(null);
               });

            modelBuilder.Entity<Qualification>(
               entity =>
               {
                   entity.ToTable("Qualification");
                   entity.HasKey(c => c.AutoId)
                       .HasName("PK_Qualification_AutoId");
               });

            modelBuilder.Entity<EmployeeSkill>(
               entity =>
               {
                   entity.ToTable("EmployeeSkill");
                   entity.HasKey(c => c.AutoId)
                       .HasName("PK_EmployeeSkill_AutoId");
               });

            modelBuilder.Entity<EmployeeCertification>(
              entity =>
              {
                  entity.ToTable("EmployeeCertification");
                  entity.HasKey(c => c.AutoId)
                      .HasName("PK_EmployeeCertification_AutoId");
              });

            modelBuilder.Entity<LanguageSkill>(
             entity =>
             {
                 entity.ToTable("LanguageSkill");
                 entity.HasKey(c => c.AutoId)
                     .HasName("PK_LanguageSkill_AutoId");
             });

            modelBuilder.Entity<FamilyMember>(
             entity =>
             {
                 entity.ToTable("FamilyMember");
                 entity.HasKey(c => c.AutoId)
                     .HasName("PK_FamilyMember_AutoId");
             });

            modelBuilder.Entity<FamilyVisa>(
             entity =>
             {
                 entity.ToTable("FamilyVisa");
                 entity.HasKey(c => c.AutoId)
                     .HasName("PK_FamilyVisa_AutoId");
             });

            modelBuilder.Entity<EmploymentHistory>(
                entity =>
                {
                    entity.ToTable("EmploymentHistory");
                    entity.HasKey(c => c.AutoId)
                        .HasName("PK_EmploymentHistory_AutoId");
                });

            modelBuilder.Entity<OtherDocument>(
                entity =>
                {
                    entity.ToTable("OtherDocument");
                    entity.HasKey(c => c.AutoId)
                        .HasName("PK_OtherDocument_AutoId");
                });

            modelBuilder.Entity<EmployeeTransaction>(
                entity =>
                {
                    entity.ToTable("EmployeeTransaction");
                    entity.HasKey(c => new { c.ActionCode, c.SectionCode, c.TransactionNo })
                        .HasName("PK_EmployeeTransaction_CompKey");
                });
            #endregion

            #region Set Employee navigation relationships                        
            modelBuilder.Entity<Employee>(
                entity =>
                {
                    entity.ToTable("Employee");

                    // Set the primary key
                    entity.HasKey(c => c.EmployeeId)
                        .HasName("PK_Employee_EmployeeId");

                    #region Set indexes
                    entity.HasIndex(e => e.EmployeeNo)
                        .HasDatabaseName("IX_Employee_EmployeeNo") 
                        .IsUnique()
                        .IsDescending();

                    entity.HasIndex(e => new { e.FirstName, e.MiddleName, e.LastName })
                        .HasDatabaseName("IX_Employee_EmpName");

                    entity.HasIndex(e => new { e.NationalityCode, e.ReligionCode, e.GenderCode, e.MaritalStatusCode })
                        .HasDatabaseName("IX_Employee_Attribute");

                    entity.HasIndex(e => new { e.HireDate, e.TerminationDate, e.DateOfConfirmation, e.DOB })
                        .HasDatabaseName("IX_Employee_Date")
                        .IsDescending();
                    #endregion

                    entity.HasOne(e => e.IdentityProof)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey<Employee>(e => e.EmployeeNo)       // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey<IdentityProof>(e => e.EmployeeNo)  // Assuming EmployeeNo is the foreign key in IdentityProof
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.Qualifications)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey(q => q.EmployeeNo)    
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.EmployeeSkills)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey(s => s.EmployeeNo)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.EmployeeCertifications)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey(c => c.EmployeeNo)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.LanguageSkills)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey(c => c.EmployeeNo)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.FamilyMembers)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey(c => c.EmployeeNo)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.FamilyVisas)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey(c => c.EmployeeNo)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.EmploymentHistories)
                       .WithOne(e => e.Employee)
                       .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                       .HasForeignKey(c => c.EmployeeNo)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.OtherDocuments)
                       .WithOne(e => e.Employee)
                       .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                       .HasForeignKey(c => c.EmployeeNo)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Cascade);

                    entity.HasMany(e => e.EmployeeTransactions)
                       .WithOne(e => e.Employee)
                       .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                       .HasForeignKey(c => c.EmployeeNo)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Cascade);
                });
            #endregion


            // Configure the Employee entity with composite key and relationship to IdentityProof
            //modelBuilder.Entity<Employee>(
            //   nestedBuilder =>
            //   {
            //        nestedBuilder.ToTable("Employee");
            //        nestedBuilder.HasKey(e => new { e.EmployeeId, e.EmployeeNo, e.HireDate })
            //            .HasName("PK_Employee_CompKeys");

            //       nestedBuilder.HasOne(e => e.IdentityProof)
            //            .WithOne(e => e.Employee)
            //            .HasPrincipalKey<Employee>(e => new { e.EmployeeId, e.EmployeeNo, e.HireDate })
            //            .HasForeignKey<IdentityProof>(e => new { e.IPEmployeeId, e.EmployeeNo, e.IPHireDate })
            //            .IsRequired()
            //            .OnDelete(DeleteBehavior.Cascade);
            //   });
        }
        #endregion
    }
}
