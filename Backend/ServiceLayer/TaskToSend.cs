using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskToSend
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        internal TaskToSend(Task task) 
        {
            this.Id = task.GetId();
            this.CreationTime = task.GetCreationTime();
            this.Title = task.GetTitle();
            this.Description = task.GetDescription();
            this.DueDate = task.GetDueDate();
        }
        public TaskToSend() { }
    }
}
