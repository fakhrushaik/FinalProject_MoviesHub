# MoviesHub — ASP.NET Core MVC Team Project  
### IT 3047C — Web Server Application Development

MoviesHub is a team-built ASP.NET Core MVC web application that showcases one of our shared hobbies: **movies**.  
The site includes custom content pages, a SQL Server LocalDB database, a movie catalog with full CRUD functionality, and a clean Bootstrap UI.

This project was created for **IT 3047C – Web Server Application Development**.

---

## Team Members

| Name | Responsibilities |
|------|------------------|
| **Anthony Than** | About Me page, UI styling, Bootstrap layout, content updates |
| **Fakhruddin Shaik** | About Me page, Home page redesign, About Movies content |
| **Advait Parab** | About Me page and additional UI/content updates, navigation updates |

**Shared Work:** Database & migrations, MoviesController, Models, CRUD pages, testing, Git workflows

Each team member has a personal “About Me” page under **Views/Home/**.

---

## Project Features

### ✔ ASP.NET Core MVC Architecture
- Organized Controllers, Models, and Views  
- View folders grouped by controller  
- Shared layout system (`_Layout.cshtml`, `_ViewStart.cshtml`, `_ViewImports.cshtml`)

### SQL Server LocalDB + Entity Framework Core
- LocalDB connection using `(LocalDB)\MSSQLLocalDB`  
- Movie table includes:
  - Title  
  - Director  
  - Genre  
  - Release Year  
  - Runtime  
  - Rating  
- EF Core Migrations used to generate database schema  
- DbContext injected using ASP.NET Core Dependency Injection

### Movie Catalog (CRUD)
- View all movies in an HTML table  
- Add a new movie  
- Edit movie details  
- Delete movies  
- View detailed movie information  
- Implemented using EF Core

### Hobby Content Pages
- **Home page** – team introduction & feature overview  
- **About Movies** – simple history of film + why we enjoy movies as a team  
- **Genres page**  
- **Top Picks page**  
- **Individual About Me pages** (one per team member)

### Bootstrap 5 UI & wwwroot Assets
- Responsive layout using Bootstrap  
- Images stored under `wwwroot/images`  
- Resume/PDF files stored under `wwwroot/files`
- Custom styling through site.css

---

## 🛠 Technology Stack

- **ASP.NET Core MVC 8.0**
- **Entity Framework Core**
- **SQL Server LocalDB**
- **Bootstrap 5**
- **C#**
- **Git / GitHub**

---
## License

This project was developed for IT 3047C - Web Server Application Development at the University of Cincinnati.
It is intended for educational use only.

