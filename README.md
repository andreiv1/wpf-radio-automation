# Radio Automation System

This is a proof of concept desktop application developed as part of a Computer-Science degree paper project in year 2023. It showcases a radio station management system, consisting of two desktop applications focused on managing media files, scheduling programs, and automated content playback.

⚠️ *Note*: This application is intended for demonstration purposes and is not a fully-fledged production solution.

## Features
### Station Management Application
The Station Management Application helps organize the station’s media content, schedule programs, and monitor performance. Users can manage audio files, plan playlists, and view key performance indicators.

![Launcher](/docs/1.png)

#### Key Features
- *Media Library*: Users can find, add, modify, and organize audio elements within the station's database (shown in the Media Library figures). Import operations display real-time progress and results

![Media Library](/docs/2.png)

![Importing media files](/docs/3.png)

- *Program Planner*: Create templates for hourly and daily broadcasts.

![Planner](/docs/4.png)

- *Scheduling*: Assign programs and playlists to the station’s schedule.

![Scheduling](/docs/44.png)

- Organize radio programs and playlists for seamless content scheduling
- View and analyze station performance metrics (e.g., play counts, airtime)

### Playout Application
The Playout Application is responsible for the automatic playback of scheduled programs. It allows operators to manage playlists and control playback during broadcasts.

![Playout App](/docs/6.png)

- Automated Playback: Plays scheduled content automatically.
- Manual Mode: Allows operators to adjust the playlist and playback in real time.


## Tech Stack
- C# / .NET Core
- WPF (Windows Presentation Foundation)
- Entity Framework Core for data access
- MariaDB/MySQL as database backend 


## Installation
1. **Clone the repository and set up MariaDB database**:
   
2. **Build the Solution in Visual Studio**:
    - Open the project in Visual Studio and build the solution.

3. **Install Entity Framework Tools**:
    - In Visual Studio, go to `Tools -> Command Line -> Developer Command Prompt`.
    - In the command prompt, install the Entity Framework CLI tools by running:
      ```bash
      dotnet tool install --global dotnet-ef
      ```

4. **Create a Migration Script**:
    - Navigate to the "RA.Database" folder:
      ```bash
      cd RA.Database
      ```
    - Generate the migration script:
      ```
      dotnet ef migrations script 0 -o MigrationScript.sql
      ```

5. **Set up the MariaDB Database**:
    - If everything runs smoothly, you should have a SQL script named _MigrationScript.sql_.
    - Open your SQL manager (e.g., MySQL Workbench, HeidiSQL) and create a database named `ra_prod1` on your MariaDB server.
    - The connection string is located in the _appsettings.json_ file in the application's build `bin` directory. Ensure it points to your `ra_prod1` database for both application (Station Management and Playout)

6. **Execute the Migration Script**:
    - In your SQL manager, execute the script created earlier (`MigrationScript.sql`) to set up the required database tables.

## Database Diagram
![Database](/docs/database.svg)