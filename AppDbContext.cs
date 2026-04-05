using Microsoft.EntityFrameworkCore;
using SmartEventManagement.Models;

namespace SmartEventManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<CommunityEvent> Events => Set<CommunityEvent>();
    public DbSet<EventSeatOption> EventSeatOptions => Set<EventSeatOption>();
    public DbSet<EventReview> EventReviews => Set<EventReview>();
    public DbSet<TicketBooking> TicketBookings => Set<TicketBooking>();
    public DbSet<GuestInquiry> GuestInquiries => Set<GuestInquiry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Member>()
            .HasIndex(m => m.Email)
            .IsUnique();

        modelBuilder.Entity<EventReview>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Reviews)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventReview>()
            .HasOne(r => r.Member)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketBooking>()
            .HasOne(b => b.Event)
            .WithMany(e => e.Bookings)
            .HasForeignKey(b => b.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TicketBooking>()
            .HasOne(b => b.Member)
            .WithMany(m => m.Bookings)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EventSeatOption>()
            .HasOne(s => s.CommunityEvent)
            .WithMany(e => e.SeatOptions)
            .HasForeignKey(s => s.CommunityEventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
