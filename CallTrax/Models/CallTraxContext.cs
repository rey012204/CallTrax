using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CallTrax.Models
{
    public partial class CallTraxContext : DbContext
    {
        public CallTraxContext()
        {
        }

        public CallTraxContext(DbContextOptions<CallTraxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Call> Call { get; set; }
        public virtual DbSet<CallFlow> CallFlow { get; set; }
        public virtual DbSet<CallFlowStep> CallFlowStep { get; set; }
        public virtual DbSet<CallFlowStepDial> CallFlowStepDial { get; set; }
        public virtual DbSet<CallFlowStepGather> CallFlowStepGather { get; set; }
        public virtual DbSet<CallFlowStepGatherRetry> CallFlowStepGatherRetry { get; set; }
        public virtual DbSet<CallFlowStepOption> CallFlowStepOption { get; set; }
        public virtual DbSet<CallFlowStepSay> CallFlowStepSay { get; set; }
        public virtual DbSet<CallFlowStepType> CallFlowStepType { get; set; }
        public virtual DbSet<CallGather> CallGather { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientContact> ClientContact { get; set; }
        public virtual DbSet<Tollfree> Tollfree { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=CallTrax;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Call>(entity =>
            {
                entity.Property(e => e.DateTimeReceived).HasColumnType("datetime");

                entity.HasOne(d => d.CallFlow)
                    .WithMany(p => p.Call)
                    .HasForeignKey(d => d.CallFlowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Call_CallFlow");
            });

            modelBuilder.Entity<CallFlow>(entity =>
            {
                entity.Property(e => e.FlowName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.CallFlow)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlow_Client");
            });

            modelBuilder.Entity<CallFlowStep>(entity =>
            {
                entity.HasOne(d => d.CallFlow)
                    .WithMany(p => p.CallFlowStep)
                    .HasForeignKey(d => d.CallFlowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStep_CallFlow");

                entity.HasOne(d => d.CallFlowStepTypeNavigation)
                    .WithMany(p => p.CallFlowStep)
                    .HasForeignKey(d => d.CallFlowStepType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStep_CallFlowStepType");
            });

            modelBuilder.Entity<CallFlowStepDial>(entity =>
            {
                entity.HasKey(e => e.CallFlowStepId);

                entity.Property(e => e.CallFlowStepId).ValueGeneratedNever();

                entity.Property(e => e.DialPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlowStep)
                    .WithOne(p => p.CallFlowStepDial)
                    .HasForeignKey<CallFlowStepDial>(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStepDial_CallFlowStep");
            });

            modelBuilder.Entity<CallFlowStepGather>(entity =>
            {
                entity.HasKey(e => e.CallFlowStepId)
                    .HasName("PK_CallFlowStepGather_1");

                entity.Property(e => e.CallFlowStepId).ValueGeneratedNever();

                entity.Property(e => e.GatherName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlowStep)
                    .WithOne(p => p.CallFlowStepGather)
                    .HasForeignKey<CallFlowStepGather>(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStepGather_CallFlowStep");
            });

            modelBuilder.Entity<CallFlowStepGatherRetry>(entity =>
            {
                entity.HasKey(e => e.CallFlowStepId);

                entity.Property(e => e.CallFlowStepId).ValueGeneratedNever();

                entity.HasOne(d => d.CallFlowStep)
                    .WithOne(p => p.CallFlowStepGatherRetry)
                    .HasForeignKey<CallFlowStepGatherRetry>(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStepGatherRetry_CallFlowStepGather");
            });

            modelBuilder.Entity<CallFlowStepOption>(entity =>
            {
                entity.Property(e => e.OptionDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.OptionValue)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlowStep)
                    .WithMany(p => p.CallFlowStepOption)
                    .HasForeignKey(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStepOption_CallFlowStepGather");
            });

            modelBuilder.Entity<CallFlowStepSay>(entity =>
            {
                entity.HasKey(e => e.CallFlowStepId)
                    .HasName("PK_CallFlowStepSay_1");

                entity.Property(e => e.CallFlowStepId).ValueGeneratedNever();

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.SayText)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlowStep)
                    .WithOne(p => p.CallFlowStepSay)
                    .HasForeignKey<CallFlowStepSay>(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStepSay_CallFlowStep");
            });

            modelBuilder.Entity<CallFlowStepType>(entity =>
            {
                entity.Property(e => e.StepTypeName)
                    .HasMaxLength(20)
                    .IsFixedLength();
            });

            modelBuilder.Entity<CallGather>(entity =>
            {
                entity.Property(e => e.GatherName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.GatherValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.Call)
                    .WithMany(p => p.CallGather)
                    .HasForeignKey(d => d.CallId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallGather_Call");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ClientContact>(entity =>
            {
                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientContact)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientContact_Client");
            });

            modelBuilder.Entity<Tollfree>(entity =>
            {
                entity.Property(e => e.TollfreeNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlow)
                    .WithMany(p => p.Tollfree)
                    .HasForeignKey(d => d.CallFlowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tollfree_CallFlow");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
