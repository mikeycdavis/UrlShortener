# URL Shortener (ASP.NET Core MVC)

## Overview

This project is a simple **URL Shortener** built with **ASP.NET Core MVC**. It reads short-code to long-URL mappings from a local text file at startup, listens on a web port, and redirects incoming requests based on the short code in the URL path.

The application is intentionally lightweight and avoids external dependencies such as databases, making it easy to run locally and ideal as a coding exercise or interview take-home.

---

## Features

* ASP.NET Core **MVC** architecture (Controllers, Services, Routing)
* File-based persistence using a local text file
* Thread-safe, in-memory URL lookup
* HTTP redirects for valid short URLs
* Clean separation of concerns using Dependency Injection

---

## How It Works

1. On application startup, the app reads a file named `urls.txt` from the project root.
2. Each line in the file defines a mapping between a short code and a long URL.
3. The mappings are loaded into memory and cached for fast lookup.
4. Incoming requests to `/{code}` are routed to a controller.
5. If the code exists, the user is redirected to the corresponding long URL.
6. If the code does not exist, a `404 Not Found` response is returned.

---

## Input File Format

The `urls.txt` file must be located in the project root directory.

Each line should follow this format:

```
<longUrl> <shortCode>
```

### Example

```
https://example.com/page1/long/url abc
https://another.com/this/is/a/very/long/one short
```

* The long URL and short code must be separated by a space
* Blank lines are ignored
* Short codes are case-insensitive

---

## Example Usage

Assuming the application is running on `https://localhost:5001`:

| Request URL | Result                                                     |
| ----------- | ---------------------------------------------------------- |
| `/abc`      | Redirects to `https://example.com/page1/long/url`          |
| `/short`    | Redirects to `https://another.com/this/is/a/very/long/one` |
| `/unknown`  | Returns `404 Not Found`                                    |

---

## Project Structure

```
UrlShortenerMvc/
│
├── Controllers/
│   └── RedirectController.cs
│
├── Services/
│   └── UrlMappingService.cs
│
├── Views/
│   └── Home/
│       └── Index.cshtml
│
├── urls.txt
├── Program.cs
└── README.md
```

---

## Running the Application

### Prerequisites

* .NET 7 or .NET 8 SDK

### Run Locally

```bash
dotnet restore
dotnet run
```

The application will start and listen on the default HTTPS port (typically `https://localhost:5001`).

---

## Design Considerations

* **MVC Routing** is used instead of Minimal APIs to demonstrate traditional ASP.NET Core patterns
* **Singleton service** is used to load mappings once and reuse them efficiently
* **In-memory dictionary** provides fast lookups with minimal complexity
* **302 redirects** are used by default (can be changed to 301 if desired)

---

## Limitations

* URL mappings are read-only at runtime
* Changes to `urls.txt` require an application restart
* No persistence beyond the local file
* No authentication or admin interface

---

## Possible Enhancements

* Hot-reload mappings using `FileSystemWatcher`
* Admin UI to manage short URLs
* POST endpoint to create new mappings
* Database-backed persistence
* Unit and integration tests
* Docker support

---

## License

This project is provided as-is for educational and demonstration purposes.
