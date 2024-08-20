using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Board
    {
        private int idCounter;
        private List<Column> columns;
        private readonly string[] DEFAULT_COLUMNS = new string[] { "backlog", "in progress", "done" };
        private const int LIMITLESS_VALUE = -1;
        private const int DEFAULT_LIMIT = LIMITLESS_VALUE;
        int columns_count;

        public Board()
        {
            idCounter = 0;
            columns = new List<Column>();
            foreach(string s in DEFAULT_COLUMNS)
            {
                columns.Add(new Column(s, DEFAULT_LIMIT));
            }
            columns_count = DEFAULT_COLUMNS.Length;
        }
        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        public void AddTask(string title, string description, DateTime DueDate)
        {
            Task task = new Task(idCounter, title, description, DueDate);
            idCounter++;
            if (columns[0].Tasks.Count == columns[0].Limit)
            {
                throw new KanbanException("Too much tasks! can't add another");
            }
            columns[0].Tasks[task.GetId()] = task;
        }
        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        public void LimitColumn(int columnOrdinal, int limit)
        {
            if(columnOrdinal < 0 || columnOrdinal>= columns_count) 
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            int tasksInOrdinal = columns[columnOrdinal].Tasks.Count;
            if(tasksInOrdinal > limit && limit!=LIMITLESS_VALUE)
            {
                throw new KanbanException("There are already more tasks then your limit!");
            }
            columns[columnOrdinal].Limit = limit;
        }
        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> An int with the specific column's limit</returns>
        public int GetColumnLimit(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            return columns[columnOrdinal].Limit;
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="columnOrdinal"> The number of the column</param>
        /// <returns>A string with the column's name</returns>
        public string GetColumnName(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" +(columns_count-1));
            }
            return columns[columnOrdinal].ColumnName;
        }
        /// <summary>
        /// This method gets all of the tasks in a specific column and board
        /// </summary>
        /// <param name="columnOrdinal">The column ID</param>
        /// <returns> A list of tasks from the specific column and board</returns>
        public List<Task> GetColumn(int columnOrdinal)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            List<Task> tasksOnColumn = new List<Task>();
            foreach (KeyValuePair<int, Task> kvp in columns[columnOrdinal].Tasks)
            {
                tasksOnColumn.Add(kvp.Value);
            }
            return tasksOnColumn;
        }
        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the task</param>
        public void UpdateTaskDueDate(int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if (columnOrdinal == columns_count-1)
            {
                throw new KanbanException("This task is done and can't be changed");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if (task == null)
            {
                throw new KanbanException("This task doesn't exist");
            }
            task.UpdateTaskDueDate(dueDate);
        }
        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(int columnOrdinal, int taskId, string title)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if (columnOrdinal == columns_count-1)
            {
                throw new KanbanException("This task is done and can't be changed");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if(task == null)
            {
                throw new KanbanException("This task doesn't exist");
            }
            task.UpdateTaskTitle(title);

        }
        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(int columnOrdinal, int taskId, string description)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if(columnOrdinal == columns_count - 1)
            {
                throw new KanbanException("This task is done and can't be changed");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if (task == null)
            {
                throw new KanbanException("This task doesn't exist");
            }
            task.UpdateTaskDescription(description);
        }
        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void AdvanceTask(int columnOrdinal, int taskId)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if(columnOrdinal == columns_count - 1)
            {
                throw new KanbanException("Can't advance more!");
            }
            Task task = columns[columnOrdinal].Tasks.GetValueOrDefault(taskId, null);
            if (task == null) {
                throw new KanbanException("This task doesn't exist!");
            }
            columns[columnOrdinal].Tasks.Remove(taskId);
            columns[columnOrdinal+1].Tasks.Add(taskId, task);
        }
        /// <summary>
        /// This method deletes a task
        /// </summary>

        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void DeleteTask(int columnOrdinal, int taskId)
        {
            if (columnOrdinal < 0 || columnOrdinal >= columns_count)
            {
                throw new KanbanException("ColumnOrdinal can be only between 0-" + (columns_count - 1));
            }
            if (!columns[columnOrdinal].Tasks.Remove(taskId))
            {
                throw new KanbanException("This task doesnt exist!");
            }

        }
    }
}
