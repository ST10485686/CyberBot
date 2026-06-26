using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CyberBotGUI
{
    public partial class MainWindow : Window
    {
        private ChatService bot;
        private List<ChatMessage> messages;

        // UPDATE THIS WITH YOUR SQL SERVER DETAILS
        private string connectionString = "Server=localhost;Database=CyberBotDB;Integrated Security=True;";

        public MainWindow()
        {
            InitializeComponent();

            bot = new ChatService(connectionString);

            messages = new List<ChatMessage>();
            ChatList.ItemsSource = messages;

            AudioService.PlayGreetingAsync();

            AddBotMessage("WELCOME TO CYBERBOT!\n\n" +
                         "I am your cybersecurity assistant with SQL Server database.\n\n" +
                         "Use the buttons above to navigate:\n" +
                         "  Home - Show welcome message\n" +
                         "  Topics - Cybersecurity topics\n" +
                         "  Quiz - Take cybersecurity quiz\n" +
                         "  Tasks - Manage your tasks (stored in SQL Server)\n" +
                         "  Activity Log - View recent actions\n" +
                         "  Help - Show all commands\n\n" +
                         "What is your name?");
        }

        private void AddBotMessage(string message)
        {
            var botMessage = ChatMessage.CreateBotMessage(message);
            messages.Add(botMessage);
            ChatList.Items.Refresh();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Home button logic here
        }

        private void BtnTopics_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Topics button logic here
        }

        private void BtnQuiz_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Quiz button logic here
        }

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Tasks button logic here
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Activity Log button logic here
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Help button logic here
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Clear button logic here
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Exit button logic here
        }

        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Quick Topic button logic here
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            // TODO: Implement InputBox key down logic here
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement Send button logic here
        }
    }
}