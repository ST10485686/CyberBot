# CyberBot - Part 2

## Overview

The CyberBot is a WPF-based desktop application that educates users about online safety through interactive conversation. Part 2 expands the console-based chatbot with a modern graphical user interface (GUI), sentiment detection, memory features, and dynamic responses.

## Author

- **Name:** Iviwe Bakaqana
- **Student Number:** ST10485686
- **Repository:** https://github.com/ST10485686/CyberBot

---

## Table of Contents

1. [Features](#features)
2. [Technologies Used](#technologies-used)
3. [Setup Instructions](#setup-instructions)
4. [How to Use](#how-to-use)
5. [Project Structure](#project-structure)
6. [Part 2 Requirements Checklist](#part-2-requirements-checklist)
7. [CI/CD Status](#cicd-status)
8. [Commit History](#commit-history)
9. [Video Presentation](#video-presentation)
10. [Author Information](#author-information)

---

## Features

### Core Features

| Feature | Description |
|---------|-------------|
| **GUI Interface** | Modern WPF window with buttons, chat display, and sentiment indicator |
| **Keyword Detection** | Recognizes cybersecurity keywords: password, scam, privacy |
| **Random Responses** | Multiple predefined responses, randomly selected for variety |
| **Memory and Recall** | Stores user name and favorite topic for personalized conversation |
| **Sentiment Detection** | Detects worried, curious, and frustrated sentiments |
| **Error Handling** | Graceful handling of unrecognized inputs with default responses |
| **Voice Greeting** | Plays welcome audio when application starts |
| **Code Optimization** | Uses dictionaries, lists, and proper OOP practices |

### Keyword Topics

| Keyword | Response Category |
|---------|-------------------|
| password | Password safety tips, strong password advice, 2FA recommendations |
| scam | Scam detection, phishing warnings, suspicious link advice |
| privacy | Privacy settings, VPN usage, data protection tips |
| phishing | Phishing attack explanation, URL verification |
| browsing | Safe browsing habits, HTTPS, public Wi-Fi warnings |

### Sentiment Detection

| Sentiment | Trigger Words | Bot Response |
|-----------|---------------|--------------|
| Worried | worried, scared, nervous, anxious, concerned | Empathetic reassurance with safety tips |
| Curious | curious, interested, wonder, tell me, explain | Encouraging with detailed explanations |
| Frustrated | frustrated, confused, annoyed, difficult, hard | Simplifying and offering step-by-step help |
| Neutral | (default) | Standard informative responses |

---

## Technologies Used

- **.NET 8.0** - Framework
- **WPF (Windows Presentation Foundation)** - GUI framework
- **C#** - Programming language
- **XAML** - UI markup language
- **System.Media** - Audio playback for voice greeting
- **GitHub Actions** - Continuous Integration
- **Git** - Version control

---

## Setup Instructions

### Prerequisites

- Windows OS (for audio and WPF compatibility)
- .NET 8.0 SDK or later
- Visual Studio 2022 (recommended)
- Git (optional, for cloning)

### Installation Steps

**Step 1: Clone the repository**

```bash
git clone https://github.com/ST10485686/CyberBot.git
cd CyberBot
