using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class User
    {
        private const int a_CHAR_VALUE = 'a';
        private const int A_CHAR_VALUE = 'A';
        private const int ZERO_CHAR_VALUE = '0';
        private const int NUMBERS = 10;
        private const int LETTERS = 26;

        private Dictionary<string, Board> boards;
        private string email;
        private string password;
        private bool loggedIn;

        /// <summary>
        /// This method gets the email of this instance
        /// </summary>
        /// <returns> A string that contains the email of the user</returns>
        public string GetEmail()
        {
            return email;
        }
        public User(string email, string password)
        {
            if (email == null)
            {
                throw new KanbanException("Email can't be null");
            }
            if(email.Length == 0)
            {
                throw new KanbanException("Email can't be empty!");
            }
            this.email = email;
            this.password = password;
            CheckPassword();
            boards = new Dictionary<string, Board>();
            loggedIn = false;
        }
        /// <summary>
        /// This method adds a board to this user
        /// </summary>
        /// <param name="boardName"> The name of the new board</param>
        public void AddBoard(string boardName) 
        {
            if (boardName == null)
            {
                throw new KanbanException("Board name can't be null");
            }
            if (boards.ContainsKey(boardName))
            {
                throw new KanbanException("A board with this name already exists!");
            }
            boards.Add(boardName, new Board());

        }
        /// <summary>
        /// This method removes a certain board from this user's boards
        /// </summary>
        /// <param name="boardName">The name of the board to be removed</param>

        public void RemoveBoard(string boardName)
        {
            if (boardName == null)
            {
                throw new KanbanException("Board name can't be null");
            }
            if (!boards.Remove(boardName))
            {
                throw new KanbanException("The user doesnt have a board with this name!");
            }
        }
        /// <summary>
        /// This method gets a board that has the same name from this user's boards
        /// </summary>
        /// <param name="boardName"> The name of the board</param>
        /// <returns> A board instance with the name</returns>

        public Board GetBoard(string boardName)
        {
            if (boardName == null)
            {
                throw new KanbanException("Board name can't be null");
            }
            Board board = boards.GetValueOrDefault(boardName,null);
            if(board == null)
            {
                throw new KanbanException("The user doesnt have a board with this name!");
            }
            return board;
        }
        /// <summary>
        /// This method gets all of the tasks from a desired column
        /// </summary>
        /// <param name="columnOrdinal"> The no. of the column </param>
        /// <returns> All the tasks from the column with the same column ordinal</returns>
        public List<Task> GetTasks(int columnOrdinal)
        {
            List<Task> tasks = new List<Task>();
            foreach(Board board in boards.Values)
            {
                List<Task> toAdd = board.GetColumn(columnOrdinal);
                foreach(Task task in toAdd)
                {
                    tasks.Add(task);
                }
            }
            return tasks;
        }
        /// <summary>
        /// This method changes the password of this instance
        /// </summary>
        /// <param name="password">The new password</param>

        public void ChangePassword(string password)
        {
            string oldPassword = this.password;
            try
            {
                this.password = password;
                CheckPassword();
            }
            catch 
            {
                this.password = oldPassword;
                throw;
            }
        }
        /// <summary>
        /// This method checks if a password is correct according to the requirements
        /// </summary>
        /// <param name="password">The password to be checked</param>
        private void CheckPassword()
        {
            if (password == null)
            {
                throw new KanbanException("Password can't be null");
            }
            if (password.Length < 6 || password.Length > 20)
            {
                throw new KanbanException("password too short\too long");
            }
            int upper = 0;
            int lower = 0;
            int number = 0;
            foreach (char c in password)
            {
                int testNumber = c - ZERO_CHAR_VALUE;
                int testLowerCase = c - a_CHAR_VALUE;
                int testUpperCase = c - A_CHAR_VALUE;
                if (testNumber>= 0 && testNumber < NUMBERS)
                {
                    number++;
                }
                if (testLowerCase>= 0 && testLowerCase<LETTERS)
                {
                    lower++;
                }
                if(testUpperCase>= 0 && testUpperCase < LETTERS)
                {
                    upper++;
                }
            }
            if (upper == 0)
            {
                throw new KanbanException("Password does not inclued a upper case letter");
            }
            if (lower == 0)
            {
                throw new KanbanException("Password does not inclued a lower case letter");
            }
            if (number == 0)
            {
                throw new KanbanException("Password does not inclued a number");
            }

        }
        /// <summary>
        /// This method checks if a user is logged in
        /// </summary>
        /// <returns>True if the user is logged in, False if not</returns>
        public bool IsLoggedIn() { return loggedIn; }

        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>The new user that was created, unless an error occurs </returns>
        public bool Login(string password)
        {
            bool success = Authenticate(password);
            if (success)
            {
                if (loggedIn)
                {
                    throw new KanbanException("User already logged in");
                }
                loggedIn = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        public void Logout()
        {
            if (loggedIn)
            {
                loggedIn = false;
            }
            else
            {
                throw new KanbanException("User is not logged in.");
            }
        }
        /// <summary>
        /// This method checks if the passwords match
        /// </summary>
        /// <param name="password"> The password of the user we want to autheticate</param>
        /// <returns> True if the passwords match, else false</returns>
        public bool Authenticate(string password)
        {
            return this.password.Equals(password);
        }
        /// <summary>
        /// This method returns the names of the user's boards
        /// </summary>
        /// <param name="email"> the email of the user</param>
        /// <returns>A list of strings with the names of the boards</returns>
        public List<string> GetBoardsNames()
        {
            return new List<string>(this.boards.Keys);
        }
    }
}
