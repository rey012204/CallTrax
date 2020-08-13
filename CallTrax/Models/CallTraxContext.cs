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
        public virtual DbSet<CallAction> CallAction { get; set; }
        public virtual DbSet<CallDirection> CallDirection { get; set; }
        public virtual DbSet<CallFlow> CallFlow { get; set; }
        public virtual DbSet<CallFlowStep> CallFlowStep { get; set; }
        public virtual DbSet<CallFlowStepDial> CallFlowStepDial { get; set; }
        public virtual DbSet<CallFlowStepGather> CallFlowStepGather { get; set; }
        public virtual DbSet<CallFlowStepGatherRetry> CallFlowStepGatherRetry { get; set; }
        public virtual DbSet<CallFlowStepOption> CallFlowStepOption { get; set; }
        public virtual DbSet<CallFlowStepSay> CallFlowStepSay { get; set; }
        public virtual DbSet<CallFlowStepType> CallFlowStepType { get; set; }
        public virtual DbSet<CallFromPhoneAddress> CallFromPhoneAddress { get; set; }
        public virtual DbSet<CallGather> CallGather { get; set; }
        public virtual DbSet<CallGatherAttempt> CallGatherAttempt { get; set; }
        public virtual DbSet<CallPhoneAddress> CallPhoneAddress { get; set; }
        public virtual DbSet<CallSurvey> CallSurvey { get; set; }
        public virtual DbSet<CallSurveyAnswer> CallSurveyAnswer { get; set; }
        public virtual DbSet<CallSurveyQuestion> CallSurveyQuestion { get; set; }
        public virtual DbSet<CallToPhoneAddress> CallToPhoneAddress { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientContact> ClientContact { get; set; }
        public virtual DbSet<SurveyQuestionType> SurveyQuestionType { get; set; }
        public virtual DbSet<Tollfree> Tollfree { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=CallTrax;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Call>(entity =>
            {
                entity.HasKey(e => e.CallSid)
                    .HasName("PK_Call_1");

                entity.Property(e => e.CallSid)
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.Property(e => e.AccountSid)
                    .IsRequired()
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.Property(e => e.ApiVersion)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.CallerName)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.DateTimeReceived).HasColumnType("datetime");

                entity.Property(e => e.ForwardedFrom)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.FromPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.ParentCallSid)
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.Property(e => e.ToPhoneNumber)
                    .IsRequired()
                    .HasColumnName("ToPhoneNUmber")
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.HasOne(d => d.DirectionNavigation)
                    .WithMany(p => p.Call)
                    .HasForeignKey(d => d.Direction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Call_CallDirection");
            });

            modelBuilder.Entity<CallAction>(entity =>
            {
                entity.Property(e => e.ActionDateTime).HasColumnType("date");

                entity.Property(e => e.ActionDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.CallSid)
                    .IsRequired()
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.HasOne(d => d.CallS)
                    .WithMany(p => p.CallAction)
                    .HasForeignKey(d => d.CallSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallAction_Call");
            });

            modelBuilder.Entity<CallDirection>(entity =>
            {
                entity.Property(e => e.CallDirectionName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();
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

                entity.Property(e => e.RetrySay)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlowStep)
                    .WithOne(p => p.CallFlowStepGatherRetry)
                    .HasForeignKey<CallFlowStepGatherRetry>(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStepGatherRetry_CallFlowStepGather");

                entity.HasOne(d => d.FailCallFlowStep)
                    .WithMany(p => p.CallFlowStepGatherRetry)
                    .HasForeignKey(d => d.FailCallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFlowStepGatherRetry_CallFlowStep");
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

            modelBuilder.Entity<CallFromPhoneAddress>(entity =>
            {
                entity.Property(e => e.CallSid)
                    .IsRequired()
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.HasOne(d => d.CallPhoneAddress)
                    .WithMany(p => p.CallFromPhoneAddress)
                    .HasForeignKey(d => d.CallPhoneAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFromPhoneAddress_CallPhoneAddress");

                entity.HasOne(d => d.CallS)
                    .WithMany(p => p.CallFromPhoneAddress)
                    .HasForeignKey(d => d.CallSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallFromPhoneAddress_Call");
            });

            modelBuilder.Entity<CallGather>(entity =>
            {
                entity.Property(e => e.CallSid)
                    .IsRequired()
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.Property(e => e.GatherValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlowStep)
                    .WithMany(p => p.CallGather)
                    .HasForeignKey(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallGather_CallFlowStep");

                entity.HasOne(d => d.CallS)
                    .WithMany(p => p.CallGather)
                    .HasForeignKey(d => d.CallSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallGather_Call");
            });

            modelBuilder.Entity<CallGatherAttempt>(entity =>
            {
                entity.Property(e => e.CallSid)
                    .IsRequired()
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.HasOne(d => d.CallFlowStep)
                    .WithMany(p => p.CallGatherAttempt)
                    .HasForeignKey(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallGatherAttempt_CallFlowStepGather");

                entity.HasOne(d => d.CallS)
                    .WithMany(p => p.CallGatherAttempt)
                    .HasForeignKey(d => d.CallSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallGatherAttempt_Call");
            });

            modelBuilder.Entity<CallPhoneAddress>(entity =>
            {
                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Country)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.State)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Zip)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<CallSurvey>(entity =>
            {
                entity.Property(e => e.CallSurveyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.CallSurvey)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallSurvey_Client");
            });

            modelBuilder.Entity<CallSurveyAnswer>(entity =>
            {
                entity.Property(e => e.CallSid)
                    .IsRequired()
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.Property(e => e.SurveyAnswer)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.CallS)
                    .WithMany(p => p.CallSurveyAnswer)
                    .HasForeignKey(d => d.CallSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallSurveyAnswer_Call");

                entity.HasOne(d => d.CallSurveyQuestion)
                    .WithMany(p => p.CallSurveyAnswer)
                    .HasForeignKey(d => d.CallSurveyQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallSurveyAnswer_CallSurveyQuestion");
            });

            modelBuilder.Entity<CallSurveyQuestion>(entity =>
            {
                entity.HasOne(d => d.CallFlowStep)
                    .WithMany(p => p.CallSurveyQuestion)
                    .HasForeignKey(d => d.CallFlowStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallSurveyQuestion_CallFlowStepGather");

                entity.HasOne(d => d.CallSurvey)
                    .WithMany(p => p.CallSurveyQuestion)
                    .HasForeignKey(d => d.CallSurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallSurveyQuestion_CallSurvey");

                entity.HasOne(d => d.QuestionTypeNavigation)
                    .WithMany(p => p.CallSurveyQuestion)
                    .HasForeignKey(d => d.QuestionType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallSurveyQuestion_SurveyQuestionType");
            });

            modelBuilder.Entity<CallToPhoneAddress>(entity =>
            {
                entity.Property(e => e.CallSid)
                    .IsRequired()
                    .HasColumnName("CallSId")
                    .HasMaxLength(34)
                    .IsFixedLength();

                entity.HasOne(d => d.CallPhoneAddress)
                    .WithMany(p => p.CallToPhoneAddress)
                    .HasForeignKey(d => d.CallPhoneAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallToPhoneAddress_CallPhoneAddress");

                entity.HasOne(d => d.CallS)
                    .WithMany(p => p.CallToPhoneAddress)
                    .HasForeignKey(d => d.CallSid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CallToPhoneAddress_Call");
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

            modelBuilder.Entity<SurveyQuestionType>(entity =>
            {
                entity.Property(e => e.SureveyQuestionTypeName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength();
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
