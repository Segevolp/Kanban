using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;
using Task = IntroSE.Kanban.Backend.BusinessLayer.Task;
using System.Numerics;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        internal readonly BoardFacade boardFacade;
        private ILog log;
        internal BoardService(UserFacade userFacade)
        {
            boardFacade = new BoardFacade(userFacade);
            log = WrapperService.log;
        }
        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string CreateBoard(string email, string name)
        {
            try
            {
                boardFacade.CreateBoard(email, name);
                log.Info($"{email} created board- {name}");
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch(KanbanException ex)
            {
                log.Error($"{email} tried to create board {name} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch(Exception ex) 
            {
                log.Error($"An unexpected error occurred: {email} tried to create board {name} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }

        }
        

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteBoard(string email, string name)
        {
            try
            {
                boardFacade.DeleteBoard(email, name);
                log.Info($"{email} deleted board- {name}");
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to delete board {name} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to delete board {name} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                boardFacade.LimitColumn(email, boardName, columnOrdinal, limit);
                return JsonSerializer.Serialize(new Response(null, null));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to limit column {columnOrdinal} in board {boardName} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to limit column {columnOrdinal} in board {boardName} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            try
            {
                int limit = boardFacade.GetColumnLimit(email, boardName, columnOrdinal);
                return JsonSerializer.Serialize(new Response(null, limit));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to get column {columnOrdinal} limit in board {boardName} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to get column {columnOrdinal} limit in board {boardName} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                string name = boardFacade.GetColumnName(email, boardName, columnOrdinal);
                return JsonSerializer.Serialize(new Response(null, name));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to get column {columnOrdinal} name in board {boardName} and got exception- " + ex.Message);
                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to get column {columnOrdinal} name in board {boardName} and got exception- " + ex.Message);
                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            try
            {
                List<Task> tasks = boardFacade.GetColumn(email, boardName, columnOrdinal);
                List<TaskToSend> tasksToSends = new List<TaskToSend>();
                foreach(Task task in tasks)
                {
                    tasksToSends.Add(new TaskToSend(task));
                }
                return JsonSerializer.Serialize(new Response(null, tasksToSends));
            }
            catch (KanbanException ex)
            {
                log.Error($"{email} tried to get all of the tasks in column {columnOrdinal} in board {boardName} and got exception- " + ex.Message);

                return JsonSerializer.Serialize(new Response(ex.Message, null));
            }
            catch (Exception ex)
            {
                log.Error($"An unexpected error occurred: {email} tried to get all of the tasks in column {columnOrdinal} in board {boardName} and got exception- " + ex.Message);

                string message = "Unexpected exception: " + ex.Message;
                return JsonSerializer.Serialize(new Response(message, null));
            }
        }
    }
}
