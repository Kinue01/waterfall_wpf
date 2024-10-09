using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace waterfall_wpf.Model;

public partial class WaterfallDbContext : DbContext
{
    public WaterfallDbContext()
    {
    }

    public WaterfallDbContext(DbContextOptions<WaterfallDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbClient> TbClients { get; set; }

    public virtual DbSet<TbCountry> TbCountries { get; set; }

    public virtual DbSet<TbEmployee> TbEmployees { get; set; }

    public virtual DbSet<TbPosition> TbPositions { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    public virtual DbSet<TbSession> TbSessions { get; set; }

    public virtual DbSet<TbTicket> TbTickets { get; set; }

    public virtual DbSet<TbTicketType> TbTicketTypes { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=172.20.105.123;Port=5432;Database=waterfall_db;Username=9po12-21-16;Password=ki9boh3Y");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbClient>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("tb_client_pkey");

            entity.ToTable("tb_client");

            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ClientCountryId).HasColumnName("client_country_id");
            entity.Property(e => e.ClientEmail)
                .HasMaxLength(320)
                .HasColumnName("client_email");
            entity.Property(e => e.ClientFirstname)
                .HasMaxLength(40)
                .HasColumnName("client_firstname");
            entity.Property(e => e.ClientLastname)
                .HasMaxLength(50)
                .HasColumnName("client_lastname");
            entity.Property(e => e.ClientMiddlename)
                .HasMaxLength(50)
                .HasColumnName("client_middlename");
            entity.Property(e => e.ClientUserId).HasColumnName("client_user_id");

            entity.HasOne(d => d.ClientCountry).WithMany(p => p.TbClients)
                .HasForeignKey(d => d.ClientCountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_client_client_country_id_fkey");

            entity.HasOne(d => d.ClientUser).WithMany(p => p.TbClients)
                .HasForeignKey(d => d.ClientUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_client_client_user_id_fkey");
        });

        modelBuilder.Entity<TbCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("tb_country_pkey");

            entity.ToTable("tb_country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(60)
                .HasColumnName("country_name");
        });

        modelBuilder.Entity<TbEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("tb_employee_pkey");

            entity.ToTable("tb_employee");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EmployeeFirstname)
                .HasMaxLength(50)
                .HasColumnName("employee_firstname");
            entity.Property(e => e.EmployeeLastname)
                .HasMaxLength(40)
                .HasColumnName("employee_lastname");
            entity.Property(e => e.EmployeeMiddlename)
                .HasMaxLength(50)
                .HasColumnName("employee_middlename");
            entity.Property(e => e.EmployeePositionId).HasColumnName("employee_position_id");
            entity.Property(e => e.EmployeeUserId).HasColumnName("employee_user_id");

            entity.HasOne(d => d.EmployeePosition).WithMany(p => p.TbEmployees)
                .HasForeignKey(d => d.EmployeePositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_employee_employee_position_id_fkey");

            entity.HasOne(d => d.EmployeeUser).WithMany(p => p.TbEmployees)
                .HasForeignKey(d => d.EmployeeUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_employee_employee_user_id_fkey");
        });

        modelBuilder.Entity<TbPosition>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("tb_position_pkey");

            entity.ToTable("tb_position");

            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .HasColumnName("position_name");
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("tb_role_pkey");

            entity.ToTable("tb_role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(40)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<TbSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("tb_session_pkey");

            entity.ToTable("tb_session");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.SessionTime).HasColumnName("session_time");
        });

        modelBuilder.Entity<TbTicket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("tb_ticket_pkey");

            entity.ToTable("tb_ticket");

            entity.Property(e => e.TicketId).HasColumnName("ticket_id");
            entity.Property(e => e.TicketChecked)
                .HasDefaultValue(false)
                .HasColumnName("ticket_checked");
            entity.Property(e => e.TicketClientId).HasColumnName("ticket_client_id");
            entity.Property(e => e.TicketDate).HasColumnName("ticket_date");
            entity.Property(e => e.TicketSessionId).HasColumnName("ticket_session_id");
            entity.Property(e => e.TicketTypeId).HasColumnName("ticket_type_id");

            entity.HasOne(d => d.TicketClient).WithMany(p => p.TbTickets)
                .HasForeignKey(d => d.TicketClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_ticket_ticket_client_id_fkey");

            entity.HasOne(d => d.TicketSession).WithMany(p => p.TbTickets)
                .HasForeignKey(d => d.TicketSessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_ticket_ticket_session_id_fkey");

            entity.HasOne(d => d.TicketType).WithMany(p => p.TbTickets)
                .HasForeignKey(d => d.TicketTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_ticket_ticket_type_id_fkey");
        });

        modelBuilder.Entity<TbTicketType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("tb_ticket_type_pkey");

            entity.ToTable("tb_ticket_type");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.TypeCost).HasColumnName("type_cost");
            entity.Property(e => e.TypeName)
                .HasMaxLength(15)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("tb_user_pkey");

            entity.ToTable("tb_user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(50)
                .HasColumnName("user_login");
            entity.Property(e => e.UserPass).HasColumnName("user_pass");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

            entity.HasOne(d => d.UserRole).WithMany(p => p.TbUsers)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tb_user_user_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
