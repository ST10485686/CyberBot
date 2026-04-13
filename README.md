# CyberBot

## Overview 
A console-based cybersecurity chatbot that educates user about online safety measures through interactive conversation, voice greeting, and visual enhancements.

 ## Features 
 -**Voice Greeting**: Plays a welcome message when the application starts
 - **ASCII Art**: Displays a cybersecurity- themed logo
 - **Interactive Chat**: Responds to questions about passwords, phishing, and safe browsing
 - **Typing Effect**: Simualtes natural conversation
 - **Colored Console UI** : Visual borders, dividers, and color coded messages
 - **Input Validation** Handles empty inputs and unrecongnized queries

   ## Technologies Used
   - C# .NET 8.0
   - System.Media (voice play back)
   - Github Actions (CI/ CD)
  
     ## Setup Instructions
     
    ### Requirements
     -Windows OS (For System.Media compatibality)
     -.NET 8.0 SDK
     - Visual Studio 2022

 ### Running the Application
    1. Clone the repository
    2. Open the solution in Visual Studio
    3. Ensure `greeting.wav` is in the output directory
    4. Press F5 to run

## Audio File Setup
The application looks for `greeting.wav` in the same folder as the executable. 
If missing, the chatbot runs without voice greeting.
   
## Usage
- Run the application and follow console prompts.
- Enter your name when prompted; ask about `password`, `phishing`, or `browsing` to get topic-specific advice.
- Type `help` to see available options and `exit` to quit.

## Project Structure
- `CyberBot/classes` — core classes such as `Chatbot`, `ResponseManager`, and `UIManager`  
- `CyberBot/Audio` — audio player interfaces and wrappers (`IAudioPlayer`, `SoundPlayerWrapper`)  
- `CyberBot/Program.cs` — application entry point  
- `screenshots` — images used by this README (`ci-workflow.png`)


## GitHub Actions CI
*The workflow builds the project on every push to ensure code compiles correctly.*

## Commit History
- Commit 1: Initial project setup
- Commit 2: Add voice greeting functionality
- Commit 3: Add ASCII art and UI formatting
- Commit 4: Implement response manager for cybersecurity topics
- Commit 5: Add typing effect and input validation
- Commit 6: Set up GitHub Actions CI workflow

## Video Presentation (unlisted video)
[Watch on Youtube](https://youtu.be/4x3xjmBaHbA)

## Author
-Iviwe Bakaqana  
-ST10485686

