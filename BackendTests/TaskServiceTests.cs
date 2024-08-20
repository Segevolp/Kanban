using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class TaskServiceTests
    {
        private string email;
        private string password;
        private  UserService userService;
        private TaskService taskService;
        private BoardService boardService;
        private const int MAX_TITLE = 50;
        private const int MAX_DESCRIPTION = 300;
        public TaskServiceTests(WrapperService wrapperService)
        {
            email = "tasktest@gmail.com";
            password = "Aa1234567";
            userService = wrapperService.userService;
            boardService = wrapperService.boardService;
            taskService = wrapperService.taskService;
        }
        public void RunTests()
        {
            Console.WriteLine("Testing UserService:");
            userService.Register(email, password);
            userService.Login(email, password);
            AddTask_BoardDoesNotExist_ErrorMessage();
            AddTask_ProperUse_EmptyResponse();
            AddTask_UserNotLoggedIn_ErrorMessage();
            AddTask_InvalidTitle_ErrorMessage();
            AddTask_InvalidDescription_ErrorMessage();
            UpdateTaskDescription_ProperUse_EmptyResponse();
            UpdateTaskDescription_UserNotLoggedIn_ErrorMessage();
            UpdateTaskDescription_InDone_ErrorMessage();
            UpdateTaskDueDate_InDone_ErrorMessage();
            UpdateTaskDueDate_ProperUse_EmptyResponse();
            UpdateTaskDueDate_UserNotLoggedIn_ErrorMessage();
            UpdateTaskTitle_ProperUse_EmptyResponse();
            UpdateTaskTitle_UserNotLoggedIn_ErrorMessage();
            UpdateTaskTitle_InDone_ErrorMessage();
            AdvanceTask_ProperUse_EmptyResponse();
            AdvanceTask_TaskInDone_ErrorMessage();
            AdvanceTask_UserNotLoggedIn_ErrorMessage();
            DeleteTask_NonSuchTask_ErrorMessage();
            DeleteTask_ProperUse_EmptyResponse();
            DeleteTask_UserNotLoggedIn_ErrorMessage();
            Console.WriteLine("");
        }


        /// <summary>
        /// This function tests Requirements 4 & 13
        /// </summary>
        public void AddTask_ProperUse_EmptyResponse()
        {
            string funcName = "AddTask_ProperUse_EmptyResponse";
            boardService.CreateBoard(email, funcName + "1");
            boardService.CreateBoard(email, funcName + "2");
            string response1 = taskService.AddTask(email, funcName + "1", "title", "description", DateTime.Now);
            string response2 = taskService.AddTask(email, funcName + "1", "title", "description", DateTime.Now);
            string response3 = taskService.AddTask(email, funcName + "2", "title", "description", DateTime.Now);
            string response4 = taskService.AddTask(email, funcName + "2", "title", "description", DateTime.Now);
            Assert.IsEmptyResponse(response1, funcName);
            Assert.IsEmptyResponse(response2, funcName);
            Assert.IsEmptyResponse(response3, funcName);
            Assert.IsEmptyResponse(response4, funcName);
        }
        /// <summary>
        /// This function tests Requirements 13
        /// </summary>
        public void AddTask_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "AddTask_UserNotLoggedIn_ErrorMessage";
            userService.Logout(email);
            boardService.CreateBoard(email, funcName + "1");
            boardService.CreateBoard(email, funcName + "2");
            string response1 = taskService.AddTask(email, funcName + "1", "title", "description", DateTime.Now);
            string response2 = taskService.AddTask(email, funcName + "1", "title", "description", DateTime.Now);
            string response3 = taskService.AddTask(email, funcName + "2", "title", "description", DateTime.Now);
            string response4 = taskService.AddTask(email, funcName + "2", "title", "description", DateTime.Now);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);
            Assert.IsErrorMessageResponse(response3, funcName);
            Assert.IsErrorMessageResponse(response4, funcName);
            userService.Login(email, password);
        }
        /// <summary>
        /// This function tests Requirement 4
        /// </summary>
        public void AddTask_InvalidTitle_ErrorMessage()
        {
            string funcName = "AddTask_InvalidTitle_ErrorMessage";
            string s = "";
            for (int i = 0; i < MAX_TITLE+1; i++)
            {
                s += "a";
            }
            string response = taskService.AddTask(email, funcName, s, "description", DateTime.Now);
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirement 4
        /// </summary>
        public void AddTask_InvalidDescription_ErrorMessage()
        {
            string funcName = "AddTask_InvalidDescription_ErrorMessage";
            string s = "";
            for (int i = 0; i <MAX_DESCRIPTION+1; i++) 
            {
                s += "a";
            }
            string response = taskService.AddTask(email, funcName, "title", s , DateTime.Now);
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirements 13
        /// </summary>
        public void AddTask_BoardDoesNotExist_ErrorMessage()
        {
            string funcName = "AddTask_BoardDoesNotExist_ErrorMessage";
            string response = taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            Assert.IsErrorMessageResponse(response, funcName);

        }
        /// <summary>
        /// This function tests Requirement 16
        /// </summary>
        public void UpdateTaskDueDate_ProperUse_EmptyResponse()
        {
            string funcName = "UpdateTaskDueDate_ProperUse_EmptyResponse";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                DateTime newDateTime = DateTime.UtcNow;
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId = tasks[0].Id;
                    taskService.UpdateTaskDueDate(email, funcName, 0, taskId, newDateTime);
                    string response = boardService.GetColumn(email, funcName, 0);
                    Response? r = JsonSerializer.Deserialize<Response>(response);
                    if (r != null)
                    {
                        tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r.ReturnValue);
                        if (tasks[0].DueDate.Equals(newDateTime))
                        {
                            return;
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");

            }
            Console.WriteLine(funcName + ": failed");
        }
        /// <summary>
        /// This function tests Requirement 15
        /// </summary>
        public void UpdateTaskDueDate_InDone_ErrorMessage()
        {
            string funcName = "UpdateTaskTitle_InDone_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            string temp = boardService.GetColumn(email, funcName, 0);
            Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
            if (temp2 != null)
            {
                TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                int taskId = tasks[0].Id;
                taskService.AdvanceTask(email, funcName, 0, taskId);
                taskService.AdvanceTask(email, funcName, 1, taskId);
                string response = taskService.UpdateTaskDueDate(email, funcName, 2, taskId, DateTime.Now);
                Assert.IsErrorMessageResponse(response, funcName);
            }
        }
        /// <summary>
        /// This function tests Requirements 16
        /// </summary>
        public void UpdateTaskDueDate_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "UpdateTaskDueDate_UserNotLoggedIn_ErrorMessage";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId = tasks[0].Id;
                    userService.Logout(email);
                    string response = taskService.UpdateTaskDueDate(email, funcName, 0, taskId, DateTime.UtcNow);
                    Assert.IsErrorMessageResponse(response, funcName);
                    userService.Login(email, password);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        /// <summary>
        /// This function tests Requirement 16
        /// </summary>
        public void UpdateTaskTitle_ProperUse_EmptyResponse()
        {
            string funcName = "UpdateTaskTitle_ProperUse_EmptyResponse";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId = tasks[0].Id;
                    taskService.UpdateTaskTitle(email, funcName, 0, taskId, "title2");
                    string response = boardService.GetColumn(email, funcName, 0);
                    Response? r = JsonSerializer.Deserialize<Response>(response);
                    if (r != null)
                    {
                        tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r.ReturnValue);
                        if (tasks[0].Title.Equals("title2"))
                        {
                            return;
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }    
        /// <summary>
        /// This function tests Requirement 15
        /// </summary>
        public void UpdateTaskTitle_InDone_ErrorMessage()
        {
            string funcName = "UpdateTaskTitle_InDone_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            string temp = boardService.GetColumn(email, funcName, 0);
            Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
            if (temp2 != null)
            {
                TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                int taskId = tasks[0].Id;
                taskService.AdvanceTask(email, funcName, 0, taskId);
                taskService.AdvanceTask(email, funcName, 1, taskId);
                string response = taskService.UpdateTaskTitle(email, funcName, 2, taskId, "new title");
                Assert.IsErrorMessageResponse(response, funcName);
            }
        }
        /// <summary>
        /// This function tests Requirements 16
        /// </summary>
        public void UpdateTaskTitle_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "UpdateTaskTitle_UserNotLoggedIn_ErrorMessage";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId = tasks[0].Id;
                    userService.Logout(email);
                    string response = taskService.UpdateTaskTitle(email, funcName, 0, taskId, "title2");
                    Assert.IsErrorMessageResponse(response, funcName);
                    userService.Login(email, password);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        /// <summary>
        /// This function tests Requirement 16
        /// </summary>
        public void UpdateTaskDescription_ProperUse_EmptyResponse()
        {
            string funcName = "UpdateTaskDescription_ProperUse_EmptyResponse";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId = tasks[0].Id;
                    taskService.UpdateTaskDescription(email, funcName, 0, taskId, "description2");
                    string response = boardService.GetColumn(email, funcName, 0);
                    Response? r = JsonSerializer.Deserialize<Response>(response);
                    if (r != null)
                    {
                        tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r.ReturnValue);
                        if (tasks[0].Description.Equals("description2"))
                        {
                            return;
                        }
                    }
                }
            } 
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        /// <summary>
        /// This function tests Requirement 15
        /// </summary>
        public void UpdateTaskDescription_InDone_ErrorMessage()
        {
            string funcName = "UpdateTaskDescription_InDone_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            string temp = boardService.GetColumn(email, funcName, 0);
            Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
            if (temp2 != null)
            {
                TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                int taskId = tasks[0].Id;
                taskService.AdvanceTask(email, funcName, 0, taskId);
                taskService.AdvanceTask(email, funcName, 1, taskId);
                string response = taskService.UpdateTaskDescription(email, funcName, 2, taskId, "new description");
                Assert.IsErrorMessageResponse(response, funcName);
            }
        }
        /// <summary>
        /// This function tests Requirements 16
        /// </summary>
        public void UpdateTaskDescription_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "UpdateTaskDescription_UserNotLoggedIn_ErrorMessage";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId = tasks[0].Id;
                    userService.Logout(email);
                    string response = taskService.UpdateTaskDescription(email, funcName, 0, taskId, "description2");
                    Assert.IsErrorMessageResponse(response, funcName);
                    userService.Login(email, password);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        /// <summary>
        /// This function tests Requirement 14
        /// </summary>
        public void AdvanceTask_ProperUse_EmptyResponse()
        {
            string funcName = "AdvanceTask_ProperUse_EmptyResponse";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId1 = tasks[0].Id;
                    int taskId2 = tasks[1].Id;
                    string response1 = taskService.AdvanceTask(email, funcName, 0, taskId1);
                    string response2 = taskService.AdvanceTask(email, funcName, 0, taskId2);
                    string response3 = taskService.AdvanceTask(email, funcName, 1, taskId1);
                    string response4 = taskService.AdvanceTask(email, funcName, 1, taskId2);
                    Assert.IsEmptyResponse(response1, funcName);
                    Assert.IsEmptyResponse(response2, funcName);
                    Assert.IsEmptyResponse(response3, funcName);
                    Assert.IsEmptyResponse(response4, funcName);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        /// <summary>
        /// This function tests Requirements 14
        /// </summary>
        public void AdvanceTask_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "AdvanceTask_UserNotLoggedIn_ErrorMessage";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId1 = tasks[0].Id;
                    userService.Logout(email);
                    string response = taskService.AdvanceTask(email, funcName, 0, taskId1);
                    Assert.IsErrorMessageResponse(response, funcName);
                    userService.Login(email, password);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        /// <summary>
        /// This function tests Requirement 14
        /// </summary>
        public void AdvanceTask_TaskInDone_ErrorMessage()
        {
            string funcName = "AdvanceTask_TaskInDone_ErrorMessage";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId1 = tasks[0].Id;
                    taskService.AdvanceTask(email, funcName, 0, taskId1);
                    taskService.AdvanceTask(email, funcName, 0, taskId1);
                    string response1 = taskService.AdvanceTask(email, funcName, 0, taskId1);
                    string response2 = taskService.AdvanceTask(email, funcName, 0, taskId1);
                    string response3 = taskService.AdvanceTask(email, funcName, 0, taskId1);
                    Assert.IsErrorMessageResponse(response1, funcName);
                    Assert.IsErrorMessageResponse(response2, funcName);
                    Assert.IsErrorMessageResponse(response3, funcName);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        public void DeleteTask_ProperUse_EmptyResponse()
        {
            string funcName = "DeleteTask_ProperUse_EmptyResponse";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId1 = tasks[0].Id;
                    int taskId2 = tasks[1].Id;
                    int taskId3 = tasks[2].Id;
                    taskService.AdvanceTask(email, funcName, 0, taskId1);
                    taskService.AdvanceTask(email, funcName, 1, taskId1);
                    taskService.AdvanceTask(email, funcName, 0, taskId2);
                    string response1 = taskService.DeleteTask(email, funcName, 2, taskId1);
                    string response2 = taskService.DeleteTask(email, funcName, 1, taskId2);
                    string response3 = taskService.DeleteTask(email, funcName, 0, taskId3);
                    Assert.IsEmptyResponse(response1, funcName);
                    Assert.IsEmptyResponse(response2, funcName);
                    Assert.IsEmptyResponse(response3, funcName);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        public void DeleteTask_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "DeleteTask_UserNotLoggedIn_ErrorMessage";
            try
            {
                boardService.CreateBoard(email, funcName);
                taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
                string temp = boardService.GetColumn(email, funcName, 0);
                Response? temp2 = JsonSerializer.Deserialize<Response>(temp);
                if (temp2 != null)
                {
                    TaskToSend[] tasks = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)temp2.ReturnValue);
                    int taskId1 = tasks[0].Id;
                    userService.Logout(email);
                    string response = taskService.DeleteTask(email, funcName, 0, taskId1);
                    Assert.IsErrorMessageResponse(response, funcName);
                    userService.Login(email, password);
                }
            }
            catch
            {
                Console.WriteLine(funcName + ": failed");
            }
        }
        public void DeleteTask_NonSuchTask_ErrorMessage()
        {
            string funcName = "DeleteTask_NonSuchTask_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response = taskService.DeleteTask(email, funcName, 0, 1234);
            Assert.IsErrorMessageResponse(response, funcName);
        }
    }
}
