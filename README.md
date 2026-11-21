# LifeManager

LifeManager is a student project combining a **.NET MAUI task-management application** with an **ASP.NET Core Web API** and a **PostgreSQL database**. It provides a practical way to organize tasks, categories, priorities, and due dates, with customizable appearance and a live weather feature.

## Table of contents

- [Features](#features)
- [Technology stack](#technology-stack)
- [Architecture](#architecture)
- [Getting started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Run the backend](#run-the-backend)
  - [Run the MAUI client](#run-the-maui-client)
- [Demonstration guide](#demonstration-guide)
  - [Architecture talking points](#architecture-talking-points)
- [Testing and limitations](#testing-and-limitations)
- [AI usage disclosure](#ai-usage-disclosure)
- [Project status](#project-status)


## Features

- **Tasks:** create, view, edit, mark complete, and delete tasks.
- **Organization:** assign a category, priority (High / Medium / Low), and optional due date; filter by category and sort tasks.
- **Top 3:** highlight three unfinished tasks based on priority and due date.
- **Categories:** create categories; duplicate category names are rejected by the API.
- **Settings:** change task font size and choose among six themes: White, Beige, Light Blue, Dark, Vampire, and Ubuntu Terminal. Settings persist between launches.
- **Weather:** retrieve current temperature and wind speed from Open-Meteo; show a wind-condition message, including a kitesurfing alert when wind exceeds 5 m/s. Weather loading and network errors are shown in the UI.

## Technology stack

| Layer | Technology |
| --- | --- |
| Client | .NET MAUI, C#, XAML |
| Client architecture | MVVM, CommunityToolkit.Mvvm, data binding, compiled bindings, converters |
| Backend | ASP.NET Core Web API |
| Data | PostgreSQL, Entity Framework Core (Code First / migrations) |
| External API | Open-Meteo weather API (JSON) |
| Dependency injection | .NET service container, typed `HttpClient` services |

## Architecture

The MAUI client communicates with the backend through HTTP services; the backend uses Entity Framework Core to read and write PostgreSQL data.

```text
MAUI Pages (Views)
       ↓ bindings / commands
ViewModels
       ↓ injected interfaces
Client Services (HTTP)
       ↓
ASP.NET Core Web API
       ↓ Entity Framework Core
PostgreSQL

WeatherService ──HTTP──> Open-Meteo
```

- **Models** represent tasks, categories, and weather data.
- **ViewModels** manage observable state and application behavior for MainPage, EditTaskPage, and SettingsPage.
- **Services and interfaces** isolate HTTP access from the ViewModels.
- **Views** contain XAML, layout, navigation, and UI-specific interactions such as prompts and alerts.
- **Dependency injection** is configured in `MauiProgram.cs`. Pages and ViewModels are transient because they hold page-specific state; typed HTTP clients use managed handlers.

Some task-card layouts are built in `MainPage.xaml.cs`. This is UI construction rather than database access; the application is not a completely code-behind-free MVVM implementation.

## Getting started

### Prerequisites

- Windows with Visual Studio and the **.NET MAUI workload** installed.
- A compatible .NET SDK for the solution's target framework.
- PostgreSQL running locally or accessible over the network.
- Internet access for the external weather service.

### Run the backend

1. Clone the repository and open the solution (`.sln`) in Visual Studio.
2. Locate the ASP.NET Core API project (`LifeManager.Api`).
3. Configure the PostgreSQL connection string **locally** using the project's existing configuration mechanism. Do not commit real passwords or other secrets.
4. Apply the existing EF Core migrations to the database. For example, from the API project directory, if `dotnet-ef` is installed and the project uses the default DbContext:

   ```powershell
   dotnet ef database update
   ```

5. Start the API and note the address displayed by Visual Studio. During development, the API has used `http://localhost:5257` and `https://localhost:7187`; verify the current launch settings rather than assuming these ports are fixed.

### Run the MAUI client

1. Check the client HTTP service configuration and ensure its API base address matches the running backend. `localhost` works for a Windows client talking to an API on the same machine, but may need adjustment on an emulator or physical device.
2. Set the MAUI project as the startup project, choose **Windows Machine**, and run it.
3. Create a task and confirm it appears. Test editing and deletion to verify that the client, API, and database are connected.

> **Setup note:** The exact connection-string location and client base-address setting should be checked against the committed source before following these steps on another machine. Database credentials are intentionally not documented here.

## Demonstration guide

Use this sequence for a short live presentation:

| Step | Demonstrate | What it shows |
| --- | --- | --- |
| 1 | Start the API and MAUI client | Separate frontend and backend |
| 2 | Create a task with a category, priority, and due date | REST API + PostgreSQL persistence |
| 3 | Edit, complete, and delete a task | CRUD operations |
| 4 | Change sorting and category filtering | ViewModel state and data binding |
| 5 | Show the Top 3 tasks | Priority-based task selection |
| 6 | Create a category; try creating the same name again | API validation and error display |
| 7 | Switch themes and change font size; restart the app | Settings stored with MAUI Preferences |
| 8 | Show current weather and wind condition | Async external REST API, JSON model mapping, m/s |
| 9 | Disconnect internet, refresh/restart weather, and interact with the UI | Loading message, network error handling, responsive UI |

### Architecture talking points

- **MVVM:** XAML binds to observable ViewModel properties; CommunityToolkit generates property notifications and commands.
- **Services:** `ITodoService`, `ICategoryService`, and `IWeatherService` keep HTTP requests out of the Views.
- **Dependency injection:** `MauiProgram.cs` registers services, pages, and ViewModels; constructors receive their dependencies.
- **Compiled bindings and converters:** `x:DataType` helps check bindings, while converters format task priority information.
- **Data storage:** Task and category changes are sent to the ASP.NET Core API, which persists them using EF Core and PostgreSQL.
- **Resilience:** Weather errors are displayed without blocking the rest of the interface. Offline task editing/synchronization is **not** a project feature.

## Testing and limitations

The application has been manually tested for task management, category filtering, theme changes, weather display, and weather failure when internet access is unavailable. In the offline weather test, an error message appeared and the application remained responsive.

Known scope limits:

- Tasks are stored through the backend; the client does not implement offline synchronization.
- The wind-condition text is a simple demonstration rule, **not** a kitesurfing safety forecast. Actual safety depends on additional conditions.
- Some UI event handlers and dynamically generated task cards remain in the View code-behind.

## AI usage disclosure

**Generative AI (ChatGPT) was used substantially during development.** It helped explain C#/.NET MAUI concepts, propose implementation steps, draft and refactor code, troubleshoot errors, review the MVVM/service/DI structure, and prepare documentation. This included assistance with task and category workflows, data binding, weather handling, and testing ideas.

The student guided the project's requirements and feature choices, integrated and adapted suggested code in Visual Studio, ran the application, investigated issues, and manually verified behavior. AI-generated suggestions were not assumed to be correct without testing; some required further debugging and changes. Accordingly, this project should be understood as **AI-assisted development**, not as work written entirely without AI support.

## Project status

University coursework project. The focus is demonstrating a working client–server application, persistence, MVVM concepts, dependency injection, and integration with an external REST API.
