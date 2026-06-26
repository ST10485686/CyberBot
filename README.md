# CyberBot - Part 3: Advanced Cybersecurity Chatbot with SQL Server

## Overview

CyberBot is a complete WPF-based cybersecurity awareness chatbot that educates users about online safety. Part 3 expands the application with advanced features including a **Task Assistant with SQL Server database**, **Cybersecurity Quiz**, **Natural Language Processing (NLP) simulation**, and **Activity Logging**.

## Author

- **Student Number:** ST10485686
- **Repository:** https://github.com/ST10485686/CyberBot

---

## Table of Contents

1. [Features](#features)
2. [Technologies Used](#technologies-used)
3. [Database Setup](#database-setup)
4. [Setup Instructions](#setup-instructions)
5. [How to Use](#how-to-use)
6. [Project Structure](#project-structure)
7. [Part 3 Requirements Checklist](#part-3-requirements-checklist)
8. [CI/CD Status](#cicd-status)
9. [Commit History](#commit-history)
10. [Video Presentation](#video-presentation)

---

## Features

### Part 1 & 2 Features (Preserved)
- Voice greeting on application start
- ASCII art header
- Personalized user interaction with name
- Keyword detection: password, scam, privacy, phishing, browsing
- Random responses using dictionaries and lists
- Memory and recall (user name and favorite topic)
- Sentiment detection (worried, curious, frustrated)
- WPF GUI with buttons and chat display

### Part 3 New Features

| Feature | Description |
|---------|-------------|
| **Task Assistant** | Add, view, complete, and delete cybersecurity tasks |
| **SQL Server Integration** | All tasks, activity logs, and quiz scores stored in SQL Server |
| **Cybersecurity Quiz** | 12 questions with immediate feedback and score tracking |
| **NLP Simulation** | Natural language understanding with string.Contains() |
| **Activity Log** | Tracks all actions with timestamps in database |
| **Navigation Menu** | Home, Topics, Quiz, Tasks, Activity Log, Help buttons |

---

## Technologies Used

| Technology | Purpose |
|------------|---------|
| .NET 8.0 | Framework |
| WPF (Windows Presentation Foundation) | GUI framework |
| C# | Programming language |
| XAML | UI markup language |
| SQL Server | Database for tasks, activity log, quiz scores |
| System.Data.SqlClient | SQL Server connectivity |
| System.Media | Audio playback |
| GitHub Actions | Continuous Integration |
| Git | Version control |

---

## Database Setup

### Step 1: Install SQL Server

1. Download SQL Server Express or Developer from [Microsoft](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Run the installer
3. Choose **Basic** or **Custom** installation
4. Note your connection details:
   - Server Name: `localhost` or `.\SQLEXPRESS`
   - Authentication: `Windows Authentication` (or SQL Server Authentication)

### Step 2: Install SQL Server Management Studio (SSMS)

1. Download SSMS from [Microsoft](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
2. Install and open SSMS
3. Connect to your SQL Server instance

### Step 3: Create Database

Run this SQL script in SSMS:

```sql
-- Create database
CREATE DATABASE CyberBotDB;
GO

USE CyberBotDB;
GO

-- Create tasks table
CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Reminder NVARCHAR(100),
    ReminderDate DATETIME,
    IsCompleted BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Create activity log table
CREATE TABLE ActivityLog (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Action NVARCHAR(255) NOT NULL,
    Details NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Create quiz scores table
CREATE TABLE QuizScores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Score INT NOT NULL,
    TotalQuestions INT NOT NULL,
    Percentage DECIMAL(5,2),
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO
