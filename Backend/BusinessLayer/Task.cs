using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Task
    {
        private int id;
        private DateTime creationTime;
        private string title;
        private string description;
        private DateTime dueDate;
        private const int MAX_TITLE_LENGTH = 50;
        private const int MAX_DESCRIPTION_LENGTH = 300;
        public Task(int id, string title, string description, DateTime dueDate)
        {
            if (title == null)
            {
                throw new KanbanException("Title can't be null!");
            }
            if (title == "" || title.Length > MAX_TITLE_LENGTH)
            {
                throw new KanbanException("Title cant be empty and has to have a maximum of " + MAX_TITLE_LENGTH + " characters");
            }
            if (description == null)
            {
                throw new KanbanException("Description can't be null!");
            }
            if (description.Length > MAX_DESCRIPTION_LENGTH)
            {
                throw new KanbanException("Description has to have a maximum of " + MAX_DESCRIPTION_LENGTH + " characters");
            }

            this.id = id;
            this.creationTime = DateTime.Now;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
        }

        public int GetId() { return id; }
        public DateTime GetCreationTime() {  return creationTime; }
        public string GetTitle() { return title; }
        public string GetDescription() { return description; }
        public DateTime GetDueDate() {  return dueDate; }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="dueDate">The new due date of the task</param>
        public void UpdateTaskDueDate(DateTime dueDate)
        {
            this.dueDate = dueDate;
        }
        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string title)
        {

            if (title == null)
            {
                throw new KanbanException("Title can't be null!");
            }
            if (title == "" || title.Length > MAX_TITLE_LENGTH)
            {
                throw new KanbanException("Title cant be empty and has to have a maximum of " + MAX_TITLE_LENGTH +" characters");
            }
            this.title = title;
        }
        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string description)
        {
            if (description == null)
            {
                throw new KanbanException("Description can't be null!");
            }
            if (description.Length > MAX_DESCRIPTION_LENGTH)
            {
                throw new KanbanException("Description has to have a maximum of " + MAX_DESCRIPTION_LENGTH + " characters");
            }

            this.description = description;
        }
    }
}
