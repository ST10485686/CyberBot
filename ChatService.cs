using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CyberBotGUI
{
    public class ChatService
    {
        // ===== MEMORY =====
        private string userName = "";
        private string favoriteTopic = "";
        private string currentTopic = "General";
        private string currentSentiment = "neutral";
        private string lastCategory = "";
        private Random random = new Random();

        // ===== DATABASE =====
        private DatabaseService db;

        // ===== QUIZ =====
        private List<QuizQuestion> quizQuestions;
        private int currentQuestionIndex = -1;
        private int quizScore = 0;
        private bool quizActive = false;

        // ===== RESPONSE DICTIONARIES =====
        private Dictionary<string, List<string>> responses;
        private Dictionary<string, List<string>> sentimentResponses;

        // ===== IN-MEMORY ACTIVITY LOG =====
        private List<ActivityEntry> activityLog = new List<ActivityEntry>();

        // ========== CONSTRUCTOR ==========
        public ChatService(string connectionString = null)
        {
            db = new DatabaseService(connectionString);

            if (db.TestConnection())
            {
                LogActivity("Database connected", "SQL Server connection successful");
            }
            else
            {
                LogActivity("Database connection failed", "Using in-memory storage only");
            }

            InitializeResponses();
            InitializeQuizQuestions();
            LogActivity("Chatbot started");
        }

        // ========== GETTERS ==========
        public string GetUserName() => userName;
        public string GetFavoriteTopic() => favoriteTopic;
        public string GetCurrentTopic() => currentTopic;
        public string GetCurrentSentiment() => currentSentiment;
        public int GetTaskCount() => GetTasksFromDB().Rows.Count;

        // ========== INITIALIZATION ==========

        private void InitializeResponses()
        {
            responses = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use strong passwords with 12+ characters including uppercase, lowercase, numbers, and symbols.",
                    "Never reuse passwords across accounts. Use a password manager.",
                    "Enable Two-Factor Authentication (2FA) on all accounts.",
                    "Avoid using personal information like birthdays or pet names in passwords."
                },
                ["scam"] = new List<string>
                {
                    "Never click suspicious links in emails or text messages.",
                    "Legitimate companies never ask for your password via email.",
                    "If something seems too good to be true, it is probably a scam.",
                    "Scammers create urgency. Always verify through official channels."
                },
                ["privacy"] = new List<string>
                {
                    "Review your privacy settings on social media regularly.",
                    "Use a VPN when connecting to public Wi-Fi.",
                    "Clear your browser cookies and cache regularly.",
                    "Check which apps have access to your location and camera."
                },
                ["phishing"] = new List<string>
                {
                    "Phishing attacks try to trick you into giving away personal information.",
                    "Hover over links before clicking to see the actual URL.",
                    "Check sender email addresses carefully for misspellings.",
                    "Never enter personal information on websites reached through email links."
                },
                ["browsing"] = new List<string>
                {
                    "Look for HTTPS in URLs when visiting websites.",
                    "Avoid public Wi-Fi for sensitive transactions.",
                    "Keep your browser updated for security patches.",
                    "Clear your browser cache and cookies regularly."
                }
            };

            sentimentResponses = new Dictionary<string, List<string>>
            {
                ["worried"] = new List<string>
                {
                    "I understand your concern. It is normal to feel worried about online security.",
                    "Your safety matters. Don't worry - I am here to help you stay protected.",
                    "Feeling worried is understandable. Let me share some reassuring tips."
                },
                ["curious"] = new List<string>
                {
                    "Great question! I am glad you are curious about cybersecurity.",
                    "That is an excellent topic to explore. Here is what you should know.",
                    "I am glad you asked. Curiosity is the first step to better online safety."
                },
                ["frustrated"] = new List<string>
                {
                    "I hear your frustration. Cybersecurity can feel overwhelming. Let me simplify this.",
                    "Take a deep breath. I will break this down into simple steps.",
                    "I understand it can be frustrating. Let me help you understand this better."
                }
            };
        }

        private void InitializeQuizQuestions()
        {
            quizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "Reply with your password", "Delete the email", "Report as phishing", "Ignore it" },
                    CorrectAnswer = 2,
                    Explanation = "Reporting phishing emails helps prevent scams. Legitimate companies never ask for passwords via email."
                },
                new QuizQuestion
                {
                    Question = "Which of the following is a strong password?",
                    Options = new List<string> { "123456", "password", "MyD0g!sC00l#2024", "qwerty" },
                    CorrectAnswer = 2,
                    Explanation = "A strong password uses 12+ characters with uppercase, lowercase, numbers, and symbols."
                },
                new QuizQuestion
                {
                    Question = "What is Two-Factor Authentication (2FA)?",
                    Options = new List<string> { "A type of password", "An extra layer of security", "A computer virus", "A browser extension" },
                    CorrectAnswer = 1,
                    Explanation = "2FA adds an extra layer of security beyond your password."
                },
                new QuizQuestion
                {
                    Question = "True or False: Public Wi-Fi is safe for online banking.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = 1,
                    Explanation = "Public Wi-Fi is not safe for banking. Hackers can intercept your data."
                },
                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Options = new List<string> { "A type of fish", "A scam to steal personal information", "A computer virus", "A password manager" },
                    CorrectAnswer = 1,
                    Explanation = "Phishing is a scam where attackers try to trick you."
                },
                new QuizQuestion
                {
                    Question = "How often should you update your passwords?",
                    Options = new List<string> { "Never", "Every 10 years", "Every 3-6 months", "Only when hacked" },
                    CorrectAnswer = 2,
                    Explanation = "Updating passwords every 3-6 months reduces risk."
                },
                new QuizQuestion
                {
                    Question = "True or False: Using the same password for multiple accounts is safe.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = 1,
                    Explanation = "Using the same password puts all your accounts at risk."
                },
                new QuizQuestion
                {
                    Question = "What should you do with suspicious email attachments?",
                    Options = new List<string> { "Open them", "Forward to friends", "Delete without opening", "Save to desktop" },
                    CorrectAnswer = 2,
                    Explanation = "Suspicious attachments may contain malware. Delete without opening."
                },
                new QuizQuestion
                {
                    Question = "Which of these is a sign of a scam?",
                    Options = new List<string> { "Professional email", "Urgent request for money", "Friendly tone", "Proper grammar" },
                    CorrectAnswer = 1,
                    Explanation = "Scammers often create urgency to make you act without thinking."
                },
                new QuizQuestion
                {
                    Question = "True or False: VPNs protect your privacy online.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = 0,
                    Explanation = "VPNs encrypt your internet traffic and hide your IP address."
                },
                new QuizQuestion
                {
                    Question = "What is social engineering?",
                    Options = new List<string> { "Engineering social media", "Manipulating people to reveal information", "Building apps", "Network engineering" },
                    CorrectAnswer = 1,
                    Explanation = "Social engineering uses psychological manipulation."
                },
                new QuizQuestion
                {
                    Question = "True or False: You should share your password with IT support.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = 1,
                    Explanation = "Legitimate IT support never asks for your password."
                }
            };
        }

        // ========== TASK METHODS WITH DATABASE ==========

        public DataTable GetTasksFromDB()
        {
            if (string.IsNullOrEmpty(userName))
                return new DataTable();
            return db.GetTasks(userName);
        }

        public string AddTaskToDB(string title, string description = "", string reminder = "",
                                   DateTime? reminderDate = null)
        {
            if (string.IsNullOrEmpty(userName))
                return "Please tell me your name first.";

            bool success = db.AddTask(userName, title, description, reminder, reminderDate);

            if (success)
            {
                LogActivity("Task added to database", $"Title: {title}");

                string response = $"Task added: '{title}'";
                if (!string.IsNullOrEmpty(description))
                    response += $"\nDescription: {description}";
                if (!string.IsNullOrEmpty(reminder))
                    response += $"\nReminder: {reminder}";
                else
                    response += $"\nWould you like to set a reminder? (Say 'remind me in X days')";

                return response;
            }
            return "Could not add task to database. Please try again.";
        }

        public string ShowTasksFromDB()
        {
            if (string.IsNullOrEmpty(userName))
                return "Please tell me your name first.";

            DataTable tasks = db.GetTasks(userName);

            if (tasks.Rows.Count == 0)
                return "You have no tasks. Say 'add task' to create one.";

            string result = "Your Cybersecurity Tasks:\n\n";
            int count = 1;
            foreach (DataRow row in tasks.Rows)
            {
                bool isCompleted = Convert.ToBoolean(row["IsCompleted"]);
                string title = row["Title"].ToString();
                string description = row["Description"].ToString();
                string reminder = row["Reminder"].ToString();
                DateTime created = Convert.ToDateTime(row["CreatedAt"]);

                result += $"{count}. {(isCompleted ? "[DONE]" : "[PENDING]")} {title}\n";
                if (!string.IsNullOrEmpty(description))
                    result += $"   Description: {description}\n";
                if (!string.IsNullOrEmpty(reminder))
                    result += $"   Reminder: {reminder}\n";
                result += $"   Created: {created:yyyy-MM-dd HH:mm}\n\n";
                count++;
            }

            LogActivity("Viewed tasks", $"{tasks.Rows.Count} tasks");
            return result;
        }

        public string CompleteTaskInDB(string input)
        {
            if (string.IsNullOrEmpty(userName))
                return "Please tell me your name first.";

            DataTable tasks = db.GetTasks(userName, false);

            foreach (DataRow row in tasks.Rows)
            {
                string title = row["Title"].ToString();
                if (input.ToLower().Contains(title.ToLower()))
                {
                    int taskId = Convert.ToInt32(row["Id"]);
                    if (db.UpdateTaskStatus(taskId, true))
                    {
                        LogActivity("Task completed", $"Task: {title}");
                        return $"Task '{title}' marked as completed!";
                    }
                }
            }

            return "Could not find a pending task matching that description.";
        }

        public string DeleteTaskFromDB(string input)
        {
            if (string.IsNullOrEmpty(userName))
                return "Please tell me your name first.";

            DataTable tasks = db.GetTasks(userName);

            foreach (DataRow row in tasks.Rows)
            {
                string title = row["Title"].ToString();
                if (input.ToLower().Contains(title.ToLower()))
                {
                    int taskId = Convert.ToInt32(row["Id"]);
                    if (db.DeleteTask(taskId))
                    {
                        LogActivity("Task deleted", $"Task: {title}");
                        return $"Task '{title}' has been deleted.";
                    }
                }
            }

            return "Could not find a task matching that description.";
        }

        public string SetReminderInDB(string input)
        {
            if (string.IsNullOrEmpty(userName))
                return "Please tell me your name first.";

            int days = 1;
            var words = input.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (int.TryParse(words[i], out int num) && i + 1 < words.Length && words[i + 1].Contains("day"))
                    days = num;
            }

            DataTable tasks = db.GetTasks(userName, false);

            foreach (DataRow row in tasks.Rows)
            {
                string reminder = row["Reminder"].ToString();
                if (string.IsNullOrEmpty(reminder))
                {
                    int taskId = Convert.ToInt32(row["Id"]);
                    string title = row["Title"].ToString();
                    DateTime reminderDate = DateTime.Now.AddDays(days);

                    if (db.UpdateTaskReminder(taskId, $"Remind in {days} days", reminderDate))
                    {
                        LogActivity("Reminder set", $"Task: {title}, Days: {days}");
                        return $"Got it! I will remind you in {days} days for task '{title}'.";
                    }
                }
            }

            return "No tasks found that need a reminder. Add a task first.";
        }

        // ========== PUBLIC METHODS ==========

        public string StartQuiz()
        {
            quizActive = true;
            currentQuestionIndex = 0;
            quizScore = 0;
            currentTopic = "QUIZ";
            LogActivity("Quiz started", $"Total: {quizQuestions.Count} questions");

            return "CYBERSECURITY QUIZ!\n\n" +
                   "Answer 12 questions. Type the number (1-4) or 'true'/'false'.\n\n" +
                   GetCurrentQuestion();
        }

        public string ShowTasks()
        {
            return ShowTasksFromDB();
        }

        public string GetActivityLog()
        {
            if (string.IsNullOrEmpty(userName))
                return "Please tell me your name first.";

            DataTable log = db.GetActivityLog(userName, 10);

            if (log.Rows.Count == 0)
                return "No activities recorded yet.";

            string result = "Recent Activity Log:\n\n";
            int count = 1;
            foreach (DataRow row in log.Rows)
            {
                string action = row["Action"].ToString();
                string details = row["Details"].ToString();
                DateTime created = Convert.ToDateTime(row["CreatedAt"]);

                result += $"{count}. {created:yyyy-MM-dd HH:mm} - {action}";
                if (!string.IsNullOrEmpty(details))
                    result += $" - {details}";
                result += "\n";
                count++;
            }

            return result;
        }

        public string GetHelpResponse()
        {
            return "COMMANDS:\n\n" +
                   "TASKS:\n  add task - [description]\n  show tasks\n  complete task [name]\n  delete task [name]\n  remind me in X days\n\n" +
                   "QUIZ:\n  start quiz\n\n" +
                   "OTHER:\n  show activity log\n  help\n\n" +
                   "TOPICS:\n  password, scam, privacy, phishing, browsing";
        }

        // ========== ACTIVITY LOG ==========

        public void LogActivity(string action, string details = "")
        {
            activityLog.Insert(0, new ActivityEntry
            {
                Timestamp = DateTime.Now,
                Action = action,
                Details = details
            });
            if (activityLog.Count > 20)
                activityLog.RemoveAt(activityLog.Count - 1);

            if (!string.IsNullOrEmpty(userName))
            {
                db.AddActivityLog(userName, action, details);
            }
        }

        // ========== QUIZ METHODS ==========

        private string GetCurrentQuestion()
        {
            if (currentQuestionIndex >= quizQuestions.Count)
                return "Quiz complete!";

            var q = quizQuestions[currentQuestionIndex];
            string result = $"Question {currentQuestionIndex + 1} of {quizQuestions.Count}:\n\n";
            result += q.Question + "\n\n";
            for (int i = 0; i < q.Options.Count; i++)
                result += $"  {i + 1}. {q.Options[i]}\n";
            return result;
        }

        private string AnswerQuizQuestion(string input)
        {
            if (!quizActive || currentQuestionIndex >= quizQuestions.Count)
                return "Quiz not active. Say 'start quiz' to begin!";

            var q = quizQuestions[currentQuestionIndex];
            int selected = -1;

            if (int.TryParse(input, out int num) && num >= 1 && num <= q.Options.Count)
                selected = num - 1;
            else if (input.ToLower() == "true")
                selected = 0;
            else if (input.ToLower() == "false")
                selected = 1;
            else
                return "Please type the number (1-4) or 'true'/'false'.";

            bool correct = selected == q.CorrectAnswer;
            if (correct) quizScore++;
            LogActivity("Quiz answer", correct ? "Correct" : "Incorrect");

            string response = correct ? "Correct!" : "Incorrect.";
            response += $"\n\n{q.Explanation}\n\n";

            currentQuestionIndex++;
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                quizActive = false;
                LogActivity("Quiz completed", $"Score: {quizScore}/{quizQuestions.Count}");

                double percentage = (double)quizScore / quizQuestions.Count * 100;
                db.SaveQuizScore(userName, quizScore, quizQuestions.Count, Math.Round(percentage, 2));

                response += GetQuizResult();
            }
            else
            {
                response += GetCurrentQuestion();
            }
            return response;
        }

        private string GetQuizResult()
        {
            int total = quizQuestions.Count;
            double percentage = (double)quizScore / total * 100;
            string feedback = percentage >= 80 ? "Excellent! You are a cybersecurity pro!" :
                              percentage >= 60 ? "Good job! Keep learning!" :
                              percentage >= 40 ? "Not bad! Review and try again." :
                              "Keep learning! Cybersecurity is important.";

            return $"\n\nQuiz Complete!\nScore: {quizScore}/{total}\nPercentage: {percentage:F1}%\n\n{feedback}";
        }

        // ========== NLP PROCESSING ==========

        private string ProcessNLP(string input)
        {
            string lowerInput = input.ToLower();

            if (lowerInput.Contains("add task") || lowerInput.Contains("new task"))
            {
                string title = input;
                foreach (var phrase in new[] { "add task", "new task", "create task", "add a task" })
                {
                    if (title.ToLower().Contains(phrase))
                        title = title.Substring(title.ToLower().IndexOf(phrase) + phrase.Length);
                }
                title = title.Trim().TrimStart('-', ':', ' ');
                if (string.IsNullOrEmpty(title))
                    return "What task would you like to add?";
                return AddTaskToDB(title);
            }

            if (lowerInput.Contains("show tasks") || lowerInput.Contains("my tasks"))
                return ShowTasksFromDB();

            if (lowerInput.Contains("complete task") || lowerInput.Contains("mark done"))
                return CompleteTaskInDB(input);

            if (lowerInput.Contains("delete task") || lowerInput.Contains("remove task"))
                return DeleteTaskFromDB(input);

            if (lowerInput.Contains("remind me in") || (lowerInput.Contains("reminder") && lowerInput.Contains("days")))
                return SetReminderInDB(input);

            if (lowerInput.Contains("start quiz") || lowerInput.Contains("take quiz") || lowerInput.Contains("quiz me"))
                return StartQuiz();

            if (lowerInput.Contains("show log") || lowerInput.Contains("activity log") || lowerInput.Contains("what have you done"))
                return GetActivityLog();

            if (lowerInput.Contains("help") || lowerInput.Contains("what can you do"))
                return GetHelpResponse();

            if (quizActive && (int.TryParse(input, out int _) || lowerInput == "true" || lowerInput == "false"))
                return AnswerQuizQuestion(input);

            return null;
        }

        // ========== MAIN RESPONSE ==========

        public string GetResponse(string userInput)
        {
            string input = userInput.ToLower();

            if (string.IsNullOrEmpty(userName))
            {
                userName = userInput;
                currentTopic = "GREETING";
                LogActivity("User registered", $"Name: {userName}");
                return $"Nice to meet you, {userName}!\n\n" +
                       "Use the buttons above to navigate:\n" +
                       "  Home - Show welcome\n" +
                       "  Topics - Cybersecurity topics\n" +
                       "  Quiz - Take cybersecurity quiz\n" +
                       "  Tasks - Manage your tasks\n" +
                       "  Activity Log - View recent actions\n\n" +
                       "Or ask about: passwords, scams, privacy, phishing, browsing";
            }

            string nlpResult = ProcessNLP(userInput);
            if (nlpResult != null) return nlpResult;

            DetectSentiment(input);
            string sentimentPrefix = GetSentimentResponse();

            foreach (var keyword in responses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastCategory = keyword;
                    currentTopic = keyword.ToUpper() + " SECURITY";
                    string response = responses[keyword][random.Next(responses[keyword].Count)];
                    if (currentSentiment != "neutral")
                        response = sentimentPrefix + "\n\n" + response;
                    LogActivity("Cybersecurity tip", $"Topic: {keyword}");
                    return response + "\n\nSay 'tell me more' for another tip!";
                }
            }

            if (input.Contains("more") || input.Contains("another"))
            {
                if (!string.IsNullOrEmpty(lastCategory) && responses.ContainsKey(lastCategory))
                    return responses[lastCategory][random.Next(responses[lastCategory].Count)] +
                           "\n\nSay 'tell me more' for another tip!";
            }

            if (input.Contains("bye") || input.Contains("goodbye") || input.Contains("exit"))
            {
                currentTopic = "GOODBYE";
                LogActivity("Goodbye", $"User: {userName}");
                return $"Goodbye, {userName}! Stay safe online!";
            }

            if (input.Contains("thank") || input.Contains("thanks"))
            {
                currentTopic = "GRATITUDE";
                return $"You are welcome, {userName}!";
            }

            currentTopic = "CONVERSATION";
            string[] defaults = {
                $"I am not sure I understand, {userName}. Say 'help' to see commands.",
                $"Try asking about passwords, scams, privacy, phishing, or browsing. Say 'help' for all commands."
            };
            return defaults[random.Next(defaults.Length)];
        }

        private void DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous"))
                currentSentiment = "worried";
            else if (input.Contains("curious") || input.Contains("interested") || input.Contains("wonder"))
                currentSentiment = "curious";
            else if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("hard"))
                currentSentiment = "frustrated";
            else
                currentSentiment = "neutral";
        }

        private string GetSentimentResponse()
        {
            if (currentSentiment != "neutral" && sentimentResponses.ContainsKey(currentSentiment))
                return sentimentResponses[currentSentiment][random.Next(sentimentResponses[currentSentiment].Count)];
            return "";
        }

        public void ResetMemory()
        {
            userName = "";
            favoriteTopic = "";
            lastCategory = "";
            currentSentiment = "neutral";
            currentTopic = "General";
            LogActivity("Memory reset", "All user data cleared");
        }
    }

    // ========== MODEL CLASSES ==========

    public class CyberTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }

    public class ActivityEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
    }
}