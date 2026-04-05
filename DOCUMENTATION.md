# System Design and Implementation Documentation

## 1. Project overview

The Smart Event Management and Ticketing System was designed for the Metropolitan Cultural Council to serve two user groups:

- Guests who need a low-friction way to explore events, view public feedback, and submit inquiries.
- Members who need full access to registration, secure sign-in, event discovery, ticket reservations, and post-event review submission.

The implementation uses ASP.NET Core MVC with a clear separation between controllers, models, views, and a service layer.

## 2. Functional coverage

### Home page

- Presents the council overview
- Highlights upcoming events
- Offers sign-in and registration entry points

### Member functions

- Register with personal details and event preferences
- Sign in through cookie authentication
- Browse and search events
- Book tickets with seat type and quantity
- Submit one review per booked event
- View bookings and recommendations from the member dashboard

### Guest functions

- Browse basic event information
- View public member reviews
- See restricted availability status only
- Register for membership
- Send inquiries to the council

## 3. Architecture

### Presentation layer

- MVC controllers handle requests and route users to the correct views.
- Razor views render the UI and enforce conditional presentation for guests versus members.

### Business layer

- `ICommunityEventService` defines the application operations.
- `CommunityEventService` holds seeded demo data and business rules for registration, booking, reviews, and inquiries.

### Domain model

Core entities include:

- `CommunityEvent`
- `Member`
- `TicketBooking`
- `EventReview`
- `GuestInquiry`
- `EventSeatOption`

### Authentication and authorization

- Cookie authentication is configured in `Program.cs`.
- Authenticated users receive member claims and gain access to booking and dashboard routes.
- Controllers such as `MembersController` and booking actions in `EventsController` are protected with `[Authorize]`.

## 4. Main workflows

### Registration workflow

1. Guest opens the registration page.
2. User enters personal details and event preferences.
3. Service validates uniqueness of the email.
4. New member is stored in memory and signed in automatically.

### Booking workflow

1. Member browses events and opens an event details page.
2. Member selects seat type and quantity.
3. Service checks remaining seat capacity.
4. Booking record is created and seat inventory is updated.

### Review workflow

1. Member opens the dashboard.
2. Member chooses a booked event to review.
3. Service verifies the member booked the event and has not already reviewed it.
4. Review is attached to the event and becomes publicly visible.

### Guest inquiry workflow

1. Guest opens the inquiry form.
2. Guest submits name, email, subject, and message.
3. Service stores the inquiry in memory.

## 5. Search and access control rules

- Guests can browse events, but event details are intentionally limited.
- Members can filter by keyword, category, date, location, and maximum price.
- Guests only see a simple status of `Available` or `Full`.
- Members see seat types, price points, and remaining seat counts.

## 6. Design choices

- An in-memory repository was used so the system can run immediately without database setup.
- A dedicated service layer keeps business logic out of controllers.
- Razor views use conditional logic to deliver different information to guests and members while keeping the route structure simple.
- The UI uses a council-themed design system with a hero landing page, event cards, dashboard panels, and mobile-friendly layouts.

## 7. Limitations and recommended future work

- Replace in-memory storage with SQL Server or SQLite using Entity Framework Core.
- Store passwords securely with hashing instead of plain text.
- Add role management for council administrators.
- Add event creation and management tools for staff.
- Add payment gateway integration and e-ticket generation.
- Add unit and integration tests.
- Add email notifications for confirmations and inquiry responses.

## 8. Folder structure

- `Controllers/` handles request orchestration
- `Models/` holds domain models and view models
- `Services/` contains the application service layer
- `Views/` contains Razor pages
- `wwwroot/` contains static assets and styles

## 9. Conclusion

The delivered system satisfies the requested member and guest scenarios in a clean ASP.NET MVC application structure. It is ready as a prototype and provides a strong foundation for moving to a production-backed council portal.
