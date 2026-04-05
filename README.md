# Smart Event Management and Ticketing System

This project is an ASP.NET Core MVC implementation of a smart event management and ticketing system for a metropolitan cultural council. It supports guest browsing, member registration and sign-in, event discovery, ticket booking, review submission, and guest inquiries.

## Technology

- ASP.NET Core MVC (.NET 10 SDK, MVC pattern in C#)
- Cookie-based authentication
- In-memory seeded repository/service layer for demo-ready behavior
- Bootstrap plus custom CSS for the UI

## Key features

- Home page with council overview and highlighted upcoming events
- Member registration with event preferences
- Member sign-in and personalized dashboard
- Event browsing and search by keyword, category, date, location, and price
- Ticket booking by seat type and quantity
- Public reviews visible to guests
- Review submission restricted to members who booked the event
- Guest inquiry form
- Guest view restriction for ticket availability and detailed event information

## Demo login

- Email: `member@council.local`
- Password: `Pass123!`

## Run locally

```powershell
dotnet run
```

Then open the local URL shown in the terminal.

## Notes

- Data is stored in memory for this prototype, so registrations, bookings, reviews, and inquiries reset when the app restarts.
- The system design and implementation notes are documented in `DOCUMENTATION.md`.
