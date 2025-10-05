﻿using System;
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

        #region Map Entities to SQL Tables
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<EmergencyContact> EmergencyContacts => Set<EmergencyContact>();
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
        public DbSet<UserDefinedCodeGroup> UserDefinedCodeGroups => Set<UserDefinedCodeGroup>();
        public DbSet<UserDefinedCode> UserDefinedCodes => Set<UserDefinedCode>();
        public DbSet<DepartmentMaster> DepartmentMasters => Set<DepartmentMaster>();
        #endregion

        #region Initialize Entities for mapping to Views/SP results 
        public DbSet<EmployeeMaster> EmployeeMasters { get; set; }
        #endregion

        #region Initialize entities    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the default schema that will apply to all entities
            modelBuilder.HasDefaultSchema("kenuser");

            // Configure SQL Server to be case-insensitive
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            //new EmployeeEntityConfig().Configure(modelBuilder.Entity<Employee>());

            #region Configure keyless models that are mapped to views or stored procedures
            modelBuilder.Entity<EmployeeMaster>().HasNoKey();
            #endregion

            #region Configure models
            modelBuilder.Entity<EmergencyContact>(
               entity =>
               {
                   entity.ToTable("EmergencyContact");
                   entity.HasKey(c => c.AutoId)
                       .HasName("PK_EmergencyContact_AutoId");

                   entity.HasIndex(e => new { e.EmployeeNo, e.ContactPerson, e.RelationCode, e.MobileNo })
                      .HasDatabaseName("IX_EmergencyContact_CompoKeys")
                      .IsUnique();
               });

            modelBuilder.Entity<IdentityProof>(
               entity =>
               {
                   entity.ToTable("IdentityProof");
                   entity.HasKey(c => c.AutoId)
                       .HasName("PK_IdentityProof_AutoId");

                   entity.HasIndex(e => new { e.EmployeeNo, e.PassportNumber, e.DateOfIssue, e.DateOfExpiry })
                       .HasDatabaseName("IX_IdentityProof_PassportInfo")
                       .IsUnique();

                   entity.HasIndex(e => new { e.EmployeeNo, e.NationalIDNumber, e.NationalIDTypeCode })
                       .HasDatabaseName("IX_IdentityProof_NatlIDDetail")
                        .IsUnique()
                       .HasFilter(null);

                   entity.HasIndex(e => new { e.EmployeeNo, e.ContractNumber, e.ContractDateOfIssue, e.ContractDateOfExpiry })
                       .HasDatabaseName("IX_IdentityProof_ContractDetail");

                   entity.HasIndex(e => new { e.EmployeeNo, e.VisaCountryCode, e.VisaNumber, e.VisaTypeCode })
                       .HasDatabaseName("IX_IdentityProof_VisaDetail")
                       .IsUnique()
                       .HasFilter(null);
               });

            modelBuilder.Entity<Qualification>(
               entity =>
               {
                   entity.ToTable("Qualification");
                   entity.HasKey(c => c.AutoId)
                       .HasName("PK_Qualification_AutoId");

                   entity.HasIndex(e => new { e.EmployeeNo, e.QualificationCode })
                      .HasDatabaseName("IX_Qualification_CompoKeys")
                      .IsUnique();
               });

            modelBuilder.Entity<EmployeeSkill>(
               entity =>
               {
                   entity.ToTable("EmployeeSkill");
                   entity.HasKey(c => c.AutoId)
                       .HasName("PK_EmployeeSkill_AutoId");

                   entity.HasIndex(e => new { e.EmployeeNo, e.SkillName })
                      .HasDatabaseName("IX_EmployeeSkill_CompoKeys")
                      .IsUnique();
               });

            modelBuilder.Entity<EmployeeCertification>(
              entity =>
              {
                  entity.ToTable("EmployeeCertification");
                  entity.HasKey(c => c.AutoId)
                      .HasName("PK_EmployeeCertification_AutoId");

                  entity.HasIndex(e => new { e.EmployeeNo, e.QualificationCode, e.FromMonthCode, e.FromYear, e.ToMonthCode, e.ToYear })
                      .HasDatabaseName("IX_EmployeeCertification_CompoKeys")
                      .IsUnique();
              });

            modelBuilder.Entity<LanguageSkill>(
             entity =>
             {
                 entity.ToTable("LanguageSkill");
                 entity.HasKey(c => c.AutoId)
                     .HasName("PK_LanguageSkill_AutoId");

                 entity.HasIndex(e => new { e.EmployeeNo, e.LanguageCode })
                      .HasDatabaseName("IX_LanguageSkill_CompoKeys")
                      .IsUnique();
             });

            modelBuilder.Entity<FamilyMember>(
                entity =>
                {
                    entity.ToTable("FamilyMember");
                    entity.HasKey(c => c.AutoId)
                        .HasName("PK_FamilyMember_AutoId");

                    entity.HasIndex(e => new { e.EmployeeNo, e.FirstName, e.LastName, e.RelationCode })
                        .HasDatabaseName("IX_FamilyMember_CompoKeys")
                        .IsUnique();

                    #region Set relationships    
                    //entity.HasMany(e => e.FamilyVisaList)
                    //    .WithOne(e => e.FamilyMember)
                    //    .HasForeignKey(c => c.FamilyId)
                    //    .IsRequired()
                    //    .OnDelete(DeleteBehavior.NoAction);
                    #endregion
                });

            modelBuilder.Entity<FamilyVisa>(
             entity =>
             {
                 entity.ToTable("FamilyVisa");
                 entity.HasKey(c => c.AutoId)
                     .HasName("PK_FamilyVisa_AutoId");

                 entity.HasIndex(e => new { e.EmployeeNo, e.VisaTypeCode, e.CountryCode, e.IssueDate, e.ExpiryDate })
                      .HasDatabaseName("IX_FamilyVisa_CompoKeys")
                      .IsUnique()
                      .HasFilter(null);
             });

            modelBuilder.Entity<EmploymentHistory>(
                entity =>
                {
                    entity.ToTable("EmploymentHistory");
                    entity.HasKey(c => c.AutoId)
                        .HasName("PK_EmploymentHistory_AutoId");

                    entity.HasIndex(e => new { e.EmployeeNo, e.CompanyName, e.Designation, e.FromDate, e.ToDate })
                      .HasDatabaseName("IX_EmploymentHistory_CompoKeys")
                      .IsUnique()
                      .HasFilter(null);
                });

            modelBuilder.Entity<OtherDocument>(
                entity =>
                {
                    entity.ToTable("OtherDocument");
                    entity.HasKey(c => c.AutoId)
                        .HasName("PK_OtherDocument_AutoId");

                    entity.HasIndex(e => new { e.EmployeeNo, e.DocumentName, e.DocumentTypeCode })
                      .HasDatabaseName("IX_OtherDocument_CompoKeys")
                      .IsUnique();
                });

            modelBuilder.Entity<EmployeeTransaction>(
                entity =>
                {
                    entity.ToTable("EmployeeTransaction");
                    entity.HasKey(c => new { c.ActionCode, c.SectionCode, c.TransactionNo })
                        .HasName("PK_EmployeeTransaction_CompKey");

                    entity.HasIndex(e => new { e.EmployeeNo, e.ActionCode, e.SectionCode, e.StatusCode, e.LastUpdateOn })
                      .HasDatabaseName("IX_EmployeeTransaction_CompoKeys")
                      .IsUnique()
                      .HasFilter(null);
                });

            modelBuilder.Entity<UserDefinedCode>(
                entity =>
                {
                    entity.ToTable("UserDefinedCode");
                    entity.HasKey(u => u.UDCId)
                        .HasName("PK_UserDefinedCode_UDCId");
                    entity.Property(u => u.IsActive)
                        .HasDefaultValue(true);
                    entity.HasIndex(e => new { e.UDCCode, e.UDCDesc1, e.UDCSpecialHandlingCode })
                      .HasDatabaseName("IX_UserDefinedCode_CompoKeys")
                      .IsUnique()
                      .HasFilter(null);
                });

            modelBuilder.Entity<DepartmentMaster>(
                entity =>
                {
                    entity.ToTable("DepartmentMaster");
                    entity.HasKey(d => d.DepartmentId)
                        .HasName("PK_DepartmentMaster_DeptId");
                    entity.Property(u => u.IsActive)
                        .HasDefaultValue(true);
                    entity.Property(u => u.CreatedAt)
                        .HasDefaultValue(DateTime.UtcNow);
                    entity.HasIndex(e => new { e.DepartmentCode, e.DepartmentName, e.GroupCode, e.SuperintendentEmpNo, e.ManagerEmpNo })
                      .HasDatabaseName("IX_DepartmentMaster_CompoKeys")
                      .IsUnique()
                      .HasFilter(null);
                });
            #endregion

            #region Set Employee navigation                         
            modelBuilder.Entity<Employee>(
                entity =>
                {
                    entity.ToTable("Employee");

                    // Set the primary key
                    entity.HasKey(c => c.EmployeeId)
                        .HasName("PK_Employee_EmployeeId");

                    #region Set default values
                    entity.Property(e => e.IsActive)
                        .HasDefaultValue(true);

                    entity.Property(e => e.PayGrade)
                        .HasDefaultValue(0);
                    #endregion

                    #region Set indexes
                    entity.HasIndex(e => new { e.EmployeeNo, e.FirstName, e.MiddleName, e.LastName })
                        .HasDatabaseName("IX_Employee_EmpName");

                    entity.HasIndex(e => new { e.EmployeeNo, e.NationalityCode, e.ReligionCode, e.MaritalStatusCode })
                        .HasDatabaseName("IX_Employee_Attribute");

                    entity.HasIndex(e => new { e.EmployeeNo, e.HireDate, e.TerminationDate, e.DateOfConfirmation, e.DOB })
                        .HasDatabaseName("IX_Employee_Date")
                        .IsDescending();
                    #endregion

                    #region Set relationships    
                    entity.HasMany(e => e.EmergencyContactList)
                        .WithOne(e => e.Employee)
                        .HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                        .HasForeignKey(c => c.EmployeeNo)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);

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
                       //.HasPrincipalKey(e => e.EmployeeNo)     // Map to EmployeeNo alternate key of Employee principal
                       .HasForeignKey(c => c.EmployeeNo)
                       .IsRequired()
                       .OnDelete(DeleteBehavior.Cascade);
                    #endregion
                });
            #endregion

            #region Set UserDefinedCodeGroup navigation
            modelBuilder.Entity<UserDefinedCodeGroup>(
                entity =>
                {
                    entity.ToTable("UserDefinedCodeGroup");
                    entity.HasKey(u => u.UDCGroupId)
                        .HasName("PK_UserDefinedCodeGroup_UDCGroupId");

                    entity.HasIndex(e => new { e.UDCGCode, e.UDCGDesc1, e.UDCGSpecialHandlingCode })
                      .HasDatabaseName("IX_UserDefinedCodeGroup_CompoKeys")
                      .IsUnique()
                      .HasFilter(null);

                    entity.HasMany(e => e.UserDefinedCodeList)
                        .WithOne(e => e.UserDefinedCodeGroup)
                        .HasForeignKey(u => u.GroupID)       // Map to UserDefinedCodeGroup.UDCGroupId
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
                });
            #endregion
        }
        #endregion
    }
}
