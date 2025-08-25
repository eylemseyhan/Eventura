# Eventura – Event and Ticket Sales Platform

Eventura is a platform where users can purchase tickets for events such as concerts, theater, stand-up shows, and children’s activities. After logging in, users can access detailed event information, buy tickets, and save their favorite events. Once a ticket is purchased, a **QR code is generated** for entry. The admin panel allows management of events, tickets, and artists, with a dynamic dashboard to monitor key metrics.

## Features

### For Users
- **Visitor Mode:** Browse events without logging in but cannot purchase tickets.  
- **User Login:** Required to view event details and buy tickets.  
- **Favorites:** Add preferred events to favorites.  
- **Account Management:** Edit profile, change password, view ticket history, etc.  
- **Ticket Purchase:** Complete payments to buy tickets.  
- **Contact:** Reach the admin in case of any issues.  
- **Location:** Events displayed on a map via Google Maps.  

### For Admin Panel
- **Event Management:** Add, edit, or delete events.  
- **Artist and Ticket Management:** Manage artists and ticket information.  
- **Dynamic Dashboard:** Displays metrics like total events and users dynamically.  
- **User Management:** Manage, update, or remove users.  

---

## Technologies Used

**Frontend:**  
Bootstrap, HTML, CSS, JavaScript, jQuery  

**Backend:**  
ASP.NET Core MVC  
Entity Framework Core (EF)  
PostgreSQL (Database)  

**Admin Panel Themes:**  
Dorne Master, Kai Admin, Darkpan  

---

## Architecture and Design Patterns

- **EF Design Pattern:** Separates business logic and database operations using Entity Framework Core.  
- **Repository Pattern:** Centralized management of database operations through a generic repository.  
- **Onion Architecture:** Ensures independent layers by inverting dependencies across application layers.  
- **Validation Rules:** Maintains the accuracy of user and event data.  
- **Unit of Work Pattern:** Ensures consistency in database operations.  
