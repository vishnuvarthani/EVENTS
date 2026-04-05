using Microsoft.EntityFrameworkCore;
using SmartEventManagement.Models;
using SmartEventManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace SmartEventManagement.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext db)
    {
        db.Database.Migrate();

        if (!db.Members.Any())
        {
            db.Members.AddRange(
                new Member
                {
                    FullName = "Aisha Perera",
                    Email = "member@council.local",
                    PasswordHash = PasswordUtility.HashPassword("Pass123!"),
                    PhoneNumber = "077 555 0144",
                    Role = "Member",
                    PreferencesSerialized = "Music,Theatre"
                },
                new Member
                {
                    FullName = "Varsha Fernando",
                    Email = "varsha@council.local",
                    PasswordHash = PasswordUtility.HashPassword("Pass123!"),
                    PhoneNumber = "071 440 2299",
                    Role = "Member",
                    PreferencesSerialized = "Festival,Music,Community"
                },
                new Member
                {
                    FullName = "Council Admin",
                    Email = "admin@council.local",
                    PasswordHash = PasswordUtility.HashPassword("Admin123!"),
                    PhoneNumber = "011 250 4400",
                    Role = "Admin",
                    PreferencesSerialized = string.Empty
                });

            db.SaveChanges();
        }

        if (!db.Events.Any())
        {
            var events = new List<CommunityEvent>
            {
                new()
                {
                    Name = "Colombo Lakefront Music Night",
                    Category = EventCategory.Music,
                    EventDate = DateTime.Today.AddDays(8).AddHours(19),
                    Venue = "Nelum Pokuna Open Air Terrace, Colombo",
                    Organizer = "Western Province Cultural Council",
                    Description = "An outdoor showcase featuring youth orchestras, fusion ensembles, and neighborhood choirs beside the Beira Lake precinct.",
                    ImageUrl = "https://images.unsplash.com/photo-1501386761578-eac5c94b800a?auto=format&fit=crop&w=1200&q=80",
                    SeatOptions =
                    [
                        new EventSeatOption { SeatType = SeatType.Standard, Price = 2500, Capacity = 120, Reserved = 45 },
                        new EventSeatOption { SeatType = SeatType.Premium, Price = 4500, Capacity = 60, Reserved = 31 },
                        new EventSeatOption { SeatType = SeatType.VIP, Price = 7500, Capacity = 20, Reserved = 10 }
                    ]
                },
                new()
                {
                    Name = "Kandy Riverside Theatre Weekend",
                    Category = EventCategory.Theatre,
                    EventDate = DateTime.Today.AddDays(15).AddHours(18),
                    Venue = "Kandy City Centre Studio Theatre, Kandy",
                    Organizer = "Hill Country Drama Collective",
                    Description = "A rotating program of contemporary plays, bilingual stage performances, and community-led backstage talks.",
                    ImageUrl = "https://images.unsplash.com/photo-1503095396549-807759245b35?auto=format&fit=crop&w=1200&q=80",
                    SeatOptions =
                    [
                        new EventSeatOption { SeatType = SeatType.Standard, Price = 1800, Capacity = 150, Reserved = 140 },
                        new EventSeatOption { SeatType = SeatType.Premium, Price = 3000, Capacity = 50, Reserved = 50 },
                        new EventSeatOption { SeatType = SeatType.VIP, Price = 5500, Capacity = 15, Reserved = 15 }
                    ]
                },
                new()
                {
                    Name = "Galle Community Sports Carnival",
                    Category = EventCategory.Sports,
                    EventDate = DateTime.Today.AddDays(22).AddHours(9),
                    Venue = "Galle Face Community Grounds, Colombo",
                    Organizer = "National Community Sports Office",
                    Description = "Family-friendly tournaments, youth clinics, and accessible sports demonstrations across a full day by the coast.",
                    ImageUrl = "https://images.unsplash.com/photo-1517649763962-0c623066013b?auto=format&fit=crop&w=1200&q=80",
                    SeatOptions =
                    [
                        new EventSeatOption { SeatType = SeatType.Standard, Price = 1200, Capacity = 300, Reserved = 121 },
                        new EventSeatOption { SeatType = SeatType.Premium, Price = 2000, Capacity = 80, Reserved = 25 }
                    ]
                },
                new()
                {
                    Name = "Jaffna Lanterns and Heritage Festival",
                    Category = EventCategory.Festival,
                    EventDate = DateTime.Today.AddDays(30).AddHours(17),
                    Venue = "Jaffna Cultural Square, Jaffna",
                    Organizer = "Northern Heritage Preservation Network",
                    Description = "A night market, storytelling circles, traditional dance, and cultural performances celebrating Sri Lankan heritage.",
                    ImageUrl = "https://images.unsplash.com/photo-1514525253161-7a46d19cd819?auto=format&fit=crop&w=1200&q=80",
                    SeatOptions =
                    [
                        new EventSeatOption { SeatType = SeatType.Standard, Price = 1000, Capacity = 500, Reserved = 210 },
                        new EventSeatOption { SeatType = SeatType.Premium, Price = 2500, Capacity = 80, Reserved = 58 }
                    ]
                }
            };

            db.Events.AddRange(events);
            db.SaveChanges();
        }

        if (!db.TicketBookings.Any())
        {
            var member = db.Members.Single(m => m.Email == "member@council.local");
            var varsha = db.Members.Single(m => m.Email == "varsha@council.local");
            var musicEvent = db.Events.Single(e => e.Name == "Colombo Lakefront Music Night");
            var festivalEvent = db.Events.Single(e => e.Name == "Jaffna Lanterns and Heritage Festival");

            db.TicketBookings.AddRange(
                new TicketBooking
                {
                    EventId = musicEvent.Id,
                    MemberId = member.Id,
                    EventName = musicEvent.Name,
                    SeatType = SeatType.Premium,
                    Quantity = 2,
                    UnitPrice = 4500,
                    BookedOn = DateTime.Today.AddDays(-2)
                },
                new TicketBooking
                {
                    EventId = festivalEvent.Id,
                    MemberId = varsha.Id,
                    EventName = festivalEvent.Name,
                    SeatType = SeatType.Standard,
                    Quantity = 3,
                    UnitPrice = 1000,
                    BookedOn = DateTime.Today.AddDays(-1)
                });

            db.SaveChanges();
        }

        if (!db.EventReviews.Any())
        {
            var aisha = db.Members.Single(m => m.Email == "member@council.local");
            var varsha = db.Members.Single(m => m.Email == "varsha@council.local");
            var eventMap = db.Events.AsNoTracking().ToDictionary(e => e.Name, e => e.Id);

            db.EventReviews.AddRange(
                new EventReview { EventId = eventMap["Colombo Lakefront Music Night"], MemberId = aisha.Id, MemberName = aisha.FullName, Rating = 5, Comment = "Loved the variety of performers and the smooth entry process. The lakefront setting felt amazing.", SubmittedOn = DateTime.Today.AddDays(-10) },
                new EventReview { EventId = eventMap["Colombo Lakefront Music Night"], MemberId = varsha.Id, MemberName = varsha.FullName, Rating = 4, Comment = "Great sound quality and a very friendly crowd. Food stalls were a nice bonus.", SubmittedOn = DateTime.Today.AddDays(-7) },
                new EventReview { EventId = eventMap["Kandy Riverside Theatre Weekend"], MemberId = aisha.Id, MemberName = aisha.FullName, Rating = 4, Comment = "Insightful performances and helpful volunteers, though parking was tight.", SubmittedOn = DateTime.Today.AddDays(-6) },
                new EventReview { EventId = eventMap["Kandy Riverside Theatre Weekend"], MemberId = varsha.Id, MemberName = varsha.FullName, Rating = 5, Comment = "One of the best local theatre weekends I have attended in Kandy. Strong acting and very organized seating.", SubmittedOn = DateTime.Today.AddDays(-3) },
                new EventReview { EventId = eventMap["Galle Community Sports Carnival"], MemberId = varsha.Id, MemberName = varsha.FullName, Rating = 4, Comment = "Well-managed family event with good energy. The youth coaching stations were especially useful.", SubmittedOn = DateTime.Today.AddDays(-4) },
                new EventReview { EventId = eventMap["Jaffna Lanterns and Heritage Festival"], MemberId = aisha.Id, MemberName = aisha.FullName, Rating = 5, Comment = "Beautiful lantern displays and strong cultural programming. A great event for families and visitors.", SubmittedOn = DateTime.Today.AddDays(-2) });

            db.SaveChanges();
        }
    }
}
