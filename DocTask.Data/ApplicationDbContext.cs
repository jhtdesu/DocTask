using DocTask.Core.Models;
using Microsoft.EntityFrameworkCore;
using Task = DocTask.Core.Models.Task;

namespace DocTask.Data;

public partial class ApplicationDbContext : DbContext
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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Taskunitassignment> Taskunitassignments { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<Unituser> Unitusers { get; set; }

    public virtual DbSet<Uploadfile> Uploadfiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.HasKey(e => e.FrequencyId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<FrequencyDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Frequency).WithMany(p => p.FrequencyDetails).HasConstraintName("fk_frequency_detail_frequency");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.IsRead).HasDefaultValueSql("0");

            entity.HasOne(d => d.Task).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkNotificationTask");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkNotificationUser");
        });

        modelBuilder.Entity<Core.Models.Org>(entity =>
        {
            entity.HasKey(e => e.OrgId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.ParentOrg).WithMany(p => p.InverseParentOrg)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkOrgParent");
        });

        modelBuilder.Entity<Period>(entity =>
        {
            entity.HasKey(e => e.PeriodId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PRIMARY");

            entity.Property(e => e.PercentageComplete).HasDefaultValueSql("0");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Period).WithMany(p => p.Progresses)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkProgressPeriod");

            entity.HasOne(d => d.Task).WithMany(p => p.Progresses).HasConstraintName("fkProgressTask");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.Progresses)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkProgressUpdatedBy");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Reminderid).HasName("PRIMARY");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reminder_ibfk_1");
        });

        modelBuilder.Entity<Reminderunit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasOne(d => d.Reminder).WithMany(p => p.Reminderunits).HasConstraintName("fk_reminder");

            entity.HasOne(d => d.Unit).WithMany(p => p.Reminderunits).HasConstraintName("fk_unit");
        });

        modelBuilder.Entity<Reportsummary>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportCreatedBy");

            entity.HasOne(d => d.Period).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportPeriod");

            entity.HasOne(d => d.ReportFileNavigation).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportFile");

            entity.HasOne(d => d.Task).WithMany(p => p.Reportsummaries)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkReportTask");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("PRIMARY");

            entity.Property(e => e.Createdat).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Percentagecomplete).HasDefaultValueSql("0");
            entity.Property(e => e.Priority).HasDefaultValueSql("'medium'");
            entity.Property(e => e.Status).HasDefaultValueSql("'pending'");

            entity.HasOne(d => d.Assignee).WithMany(p => p.TaskAssignees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkTaskAssignee");

            entity.HasOne(d => d.Assigner).WithMany(p => p.TaskAssigners)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkTaskAssigner");

            entity.HasOne(d => d.AttachedFileNavigation).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkTaskAttachedFile");

            entity.HasOne(d => d.Frequency).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_taskitem_frequency");

            entity.HasOne(d => d.Org).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkTaskOrg");

            entity.HasOne(d => d.Period).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkTaskPeriod");

            entity.HasMany(d => d.Users).WithMany(p => p.Tasks)
                .UsingEntity<Dictionary<string, object>>(
                    "Taskassignee",
                    r => r.HasOne<User>().WithMany()
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
            entity.HasKey(e => e.TaskUnitAssignmentId).HasName("PRIMARY");

            entity.HasOne(d => d.Task).WithMany(p => p.Taskunitassignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskunitassignment_ibfk_1");

            entity.HasOne(d => d.Unit).WithMany(p => p.Taskunitassignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("taskunitassignment_ibfk_2");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PRIMARY");

            entity.Property(e => e.Type).HasDefaultValueSql("'official'");

            entity.HasOne(d => d.Org).WithMany(p => p.Units).HasConstraintName("fkUnitOrg");

            entity.HasOne(d => d.UnitParentNavigation).WithMany(p => p.InverseUnitParentNavigation)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkUnitParent");
        });

        modelBuilder.Entity<Unituser>(entity =>
        {
            entity.HasKey(e => e.UnitUserId).HasName("PRIMARY");

            entity.HasOne(d => d.Unit).WithMany(p => p.Unitusers).HasConstraintName("fkUnitUserUnit");

            entity.HasOne(d => d.User).WithMany(p => p.Unitusers).HasConstraintName("fkUnitUserUser");
        });

        modelBuilder.Entity<Uploadfile>(entity =>
        {
            entity.HasKey(e => e.FileId).HasName("PRIMARY");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.Uploadfiles)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fkUploadFileUploadedBy");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Role).HasDefaultValueSql("0");

            entity.HasOne(d => d.Org).WithMany(p => p.Users).HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.Position).WithMany(p => p.Users).HasConstraintName("user_ibfk_1");

            entity.HasOne(d => d.Unit).WithMany(p => p.Users).HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.UnitUser).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_user_unitUser");

            entity.HasOne(d => d.UserParentNavigation).WithMany(p => p.InverseUserParentNavigation).HasConstraintName("user_ibfk_4");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Createdat).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Role).WithMany(p => p.Userroles).HasConstraintName("fk_userrole_role");

            entity.HasOne(d => d.User).WithMany(p => p.Userroles).HasConstraintName("fk_userrole_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
