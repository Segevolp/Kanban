using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade
    {
        private const int IN_PROGRESS_COLUMN_ORDINAL = 1;
        private Dictionary<string, User> users;
        public UserFacade()
        {
            users = new Dictionary<string, User>();
        }
        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The new user that was created, unless an error occurs </returns>

        public User Register(string email, string password)
        {
            if(users.ContainsKey(email))
            {
                throw new KanbanException("User already exists in the system.");
            }
            User newUser = new User(email, password);
            users.Add(email, newUser);
            return newUser;
        }
        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>The new user that was created, unless an error occurs </returns>
        public User Login(string email, string password)
        {
            User user = GetUser(email);

            if (user.Login(password))
            {
                return user;
            }
            return null;

        }
        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        public void Logout(string email)
        {
            User user = GetUser(email);

            user.Logout();
        }
        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A list of tasks of the user with this email</returns>
       
        public List<Task> InProgressTasks(string email)
        {
            User user = GetUser(email);
            if(!user.IsLoggedIn())
            {
                throw new KanbanException("User must be logged in!");
            }
            return user.GetTasks(IN_PROGRESS_COLUMN_ORDINAL);
        }
        /// <summary>
        /// This method checks if a user is logged in
        /// </summary>
        /// <returns>True if the user is logged in, False if not</returns>
        public bool IsLoggedIn(string email)
        {
            User user = GetUser(email);
            return user.IsLoggedIn();
        }
        /// <summary> 
        /// This method let a user change password.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="oldPassword">Current user password.</param>
        /// <param name="newPassword">New user password</param>
        public void ChangePassword(string email, string oldPassword, string newPassword)
        {
            User user = GetUser(email);
            if(!user.IsLoggedIn())
            {
                throw new KanbanException("User is not logged in");
            }
            if(!user.Authenticate(oldPassword))
            {
                throw new KanbanException("wrong old password");
            }
            user.ChangePassword(newPassword);

        }
        /// <summary>
        /// This method gets a user by email
        /// </summary>
        /// <param name="email"> The email of the user to be returned</param>
        /// <returns> A user that registered with the same email</returns>
        public User GetUser(string email)
        {
            if (email == null)
            {
                throw new KanbanException("Email can't be null");
            }
            User user = users.GetValueOrDefault(email, null);
            if (user == null)
            {
                throw new KanbanException("This user doesn't exist in the system!");
            }
            return user;
        }

        /// <summary>
        /// This method returns the names of the user's boards
        /// </summary>
        /// <param name="email"> the email of the user</param>
        /// <returns>A list of strings with the names of the boards</returns>
        public List<string> GetBoardsNames(string email)
        {
            User user = GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User is not logged in");
            }
            return user.GetBoardsNames();

        }
    }
}
