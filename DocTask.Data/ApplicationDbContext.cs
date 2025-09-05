using DocTask.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Data;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Frequency> Frequencies { get; set; }

    public virtual DbSet<FrequencyDetail> FrequencyDetails { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Core.Models.Org> Orgs { get; set; }

    public virtual DbSet<Period> Periods { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Progress> Progresses { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<Reminderunit> Reminderunits { get; set; }

    public virtual DbSet<Reportsummary> Reportsummaries { get; set; }

    public new virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Taskunitassignment> Taskunitassignments { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<Unituser> Unitusers { get; set; }

    public virtual DbSet<Uploadfile> Uploadfiles { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure Identity tables to use custom names
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().ToTable("AspNetRoles");
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().ToTable("AspNetUserRoles");
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>().ToTable("AspNetUserTokens");
        modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
        
        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.HasKey(e => e.FrequencyId).HasName("PK_Frequency");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<FrequencyDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_FrequencyDetail");

            entity.HasOne(d => d.Frequency).WithMany(p => p.FrequencyDetails).HasConstraintName("fk_frequency_detail_frequency");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK_Notification");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.IsRead).HasDefaultValueSql("0");

            entity.HasOne(d => d.Task).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkNotificationTask");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkNotificationUser");
        });

        modelBuilder.Entity<Core.Models.Org>(entity =>
        {
            entity.HasKey(e => e.OrgId).HasName("PK_Org");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.ParentOrg).WithMany(p => p.InverseParentOrg)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkOrgParent");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.HasKey(e => e.PeriodId).HasName("PK_Period");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK_Position");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK_Progress");

            entity.Property(e => e.PercentageComplete).HasDefaultValueSql("0");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Period).WithMany(p => p.Progresses)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkProgressPeriod");

            entity.HasOne(d => d.Task).WithMany(p => p.Progresses).HasConstraintName("fkProgressTask");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.Progresses)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkProgressUpdatedBy");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Reminderid).HasName("PK_Reminder");

            entity.Property(e => e.Createdat).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Isauto).HasDefaultValueSql("0");
            entity.Property(e => e.Isnotified).HasDefaultValueSql("0");
            entity.Property(e => e.Triggertime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Reminders).HasConstraintName("reminder_ibfk_3");

            entity.HasOne(d => d.Notification).WithMany(p => p.Reminders).HasConstraintName("reminder_ibfk_4");

            entity.HasOne(d => d.Period).WithMany(p => p.Reminders).HasConstraintName("reminder_ibfk_2");

            entity.HasOne(d => d.Task).WithMany(p => p.Reminders)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("reminder_ibfk_1");
        });

        modelBuilder.Entity<Reminderunit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Reminderunit");

            entity.HasOne(d => d.Reminder).WithMany(p => p.Reminderunits).HasConstraintName("fk_reminder");

            entity.HasOne(d => d.Unit).WithMany(p => p.Reminderunits).HasConstraintName("fk_unit");
        });

        modelBuilder.Entity<Reportsummary>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK_Reportsummary");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkReportCreatedBy");

            entity.HasOne(d => d.Period).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkReportPeriod");

            entity.HasOne(d => d.ReportFileNavigation).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkReportFile");

            entity.HasOne(d => d.Task).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkReportTask");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("PK_Role");

            entity.Property(e => e.Createdat).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK_Task");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Percentagecomplete).HasDefaultValueSql("0");
            entity.Property(e => e.Priority).HasDefaultValueSql("'medium'");
            entity.Property(e => e.Status).HasDefaultValueSql("'pending'");

            entity.HasOne(d => d.Assignee).WithMany(p => p.TaskAssignees)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkTaskAssignee");

            entity.HasOne(d => d.Assigner).WithMany(p => p.TaskAssigners)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkTaskAssigner");

            entity.HasOne(d => d.AttachedFileNavigation).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkTaskAttachedFile");

            entity.HasOne(d => d.Frequency).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fk_taskitem_frequency");

            entity.HasOne(d => d.Org).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkTaskOrg");

            entity.HasOne(d => d.Period).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkTaskPeriod");

            entity.HasMany(d => d.Users).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "Taskassignee",
                    r => r.HasOne<ApplicationUser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("taskassignees_ibfk_2"),
                    l => l.HasOne<Task>().WithMany()
                        .HasForeignKey("TaskId")
                        .HasConstraintName("taskassignees_ibfk_1"),
                    j =>
                    {
                        j.HasKey("TaskId", "UserId").HasName("PRIMARY");
                        j.ToTable("taskassignees");
                        j.HasIndex(new[] { "UserId" }, "UserId");
                    });
        });

        modelBuilder.Entity<Taskunitassignment>(entity =>
        {
            entity.HasKey(e => e.TaskUnitAssignmentId).HasName("PK_Taskunitassignment");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskunitassignments)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("taskunitassignment_ibfk_1");

            entity.HasOne(d => d.Unit).WithMany(p => p.Taskunitassignments)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("taskunitassignment_ibfk_2");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK_Unit");

            entity.Property(e => e.Type).HasDefaultValueSql("'official'");

            entity.HasOne(d => d.Org).WithMany(p => p.Units).HasConstraintName("fkUnitOrg");

            entity.HasOne(d => d.UnitParentNavigation).WithMany(p => p.InverseUnitParentNavigation)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkUnitParent");
        });

        modelBuilder.Entity<Unituser>(entity =>
        {
            entity.HasKey(e => e.UnitUserId).HasName("PK_Unituser");

            entity.HasOne(d => d.Unit).WithMany(p => p.Unitusers).HasConstraintName("fkUnitUserUnit");

            entity.HasOne(d => d.User).WithMany(p => p.Unitusers).HasConstraintName("fkUnitUserUser");
        });

        modelBuilder.Entity<Uploadfile>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PK_Uploadfile");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Uploadfiles)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fkUploadFileUploadedBy");
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Role).HasDefaultValueSql("0");

            entity.HasOne(d => d.Org).WithMany(p => p.Users).HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.Position).WithMany(p => p.Users).HasConstraintName("user_ibfk_1");

            entity.HasOne(d => d.Unit).WithMany(p => p.Users).HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.UnitUser).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("fk_user_unitUser");

            entity.HasOne(d => d.UserParentNavigation).WithMany(p => p.InverseUserParentNavigation)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("user_ibfk_4");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Userrole");

            entity.Property(e => e.Createdat).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Role).WithMany(p => p.Userroles).HasConstraintName("fk_userrole_role");

            entity.HasOne(d => d.User).WithMany(p => p.Userroles).HasConstraintName("fk_userrole_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
