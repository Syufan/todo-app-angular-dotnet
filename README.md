# todo-app-angular-dotnet

This is a full-stack TODO list application built with Angular 20 and .NET 9 Web API.


## Features

This application implements the core features of a basic TODO list system:

- View all TODO items (loaded by default)
- Add a new TODO item via form input
- Delete an existing TODO item


## Stack & Versions

- **Frontend**: Angular 20 (May 2025 release)
- **Backend**: ASP.NET Core Web API (.NET 9 STS)

> .NET 9 is the latest release, offering the newest features.  
> For longer-term support, .NET 8 is currently the LTS version.


## Data Model

```mermaid
classDiagram
  class TodoItem {
    int Id
    string Title
  }


## How to Run