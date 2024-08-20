using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardFacade
    {

        private UserFacade userFacade;

        /// <summary>
        /// Builder
        /// </summary>
        public BoardFacade(UserFacade userFacade)
        {
            this.userFacade = userFacade;
        }
        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        public void CreateBoard(string email, string name)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            user.AddBoard(name);
        }
        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        public void DeleteBoard(string email, string name)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            user.RemoveBoard(name);
        }
        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        public void LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.LimitColumn(columnOrdinal, limit);
        }
        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> An int with the specific column's limit</returns>

        public int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            return board.GetColumnLimit(columnOrdinal);
        }
        /// <summary>
        ///  This method gets the name of the column of a specific board of a specific user
        /// </summary>
        /// <param name="email"> The email of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal"> The number of the column</param>
        /// <returns>A string with The name of the column</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            return board.GetColumnName(columnOrdinal);

        }
        /// <summary>
        /// This method gets all of the tasks in a specific column and board
        /// </summary>
        /// <param name="email"> The email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID</param>
        /// <returns> A list of tasks from the specific column and board</returns>
        public List<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            return board.GetColumn(columnOrdinal);
        }
        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        public void AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.AddTask(title, description, dueDate);
        }
        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the task</param>
        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.UpdateTaskDueDate(columnOrdinal,taskId,dueDate);
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.UpdateTaskTitle(columnOrdinal, taskId, title);
        }
        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.UpdateTaskDescription(columnOrdinal, taskId, description);
        }
        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.AdvanceTask(columnOrdinal, taskId);
        }
        /// <summary>
        /// This method deletes a task
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void DeleteTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            User user = userFacade.GetUser(email);
            if (!user.IsLoggedIn())
            {
                throw new KanbanException("User isn't logged in");
            }
            Board board = user.GetBoard(boardName);
            board.DeleteTask(columnOrdinal, taskId);
        }
    }
}
