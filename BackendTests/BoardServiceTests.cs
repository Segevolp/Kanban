using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class BoardServiceTests
    {
        private string email;
        private string password;
        private UserService userService;
        private TaskService taskService;
        private BoardService boardService;
        private readonly string[] COLUMNS_NAMES = new string[] { "backlog", "in progress", "done" };
        public BoardServiceTests(WrapperService wrapperService)
        {
            email = "boardtest@gmail.com";
            password = "Aa1234567";
            userService = wrapperService.userService;
            boardService = wrapperService.boardService;
            taskService = wrapperService.taskService;
        }
        public void RunTests()
        {
            Console.WriteLine("Testing BoardService:");
            userService.Register(email, password);
            CreateBoard_BoardAlreadyExists_ErrorMesssage();
            CreateBoard_ProperUse_EmptyResponse();
            CreateBoard_LimitlessInit_MinusOne();
            CreateBoard_UserNotLoggedIn_ErrorMessage();
            CreateBoard_SameNameDifferentAccounts_EmptyResponse();
            DeleteBoard_BoardDoesNotExist_ErrorMesssage();
            DeleteBoard_ProperUse_EmptyResponse();
            DeleteBoard_UserNotLoggedIn_ErrorMessage();
            LimitColumn_InvalidColumnOrdinal_ErrorMessage();
            LimitColumn_InvalidNumber_ErrorMessage();
            LimitColumn_Limitless_EmptyResponse();
            LimitColumn_ProperUse_EmptyResponse();
            GetColumnLimit_InvalidBoard_ErrorMessage();
            GetColumnLimit_InvalidColumnOrdinal_ErrorMessage();
            GetColumnLimit_ProperUse_Limit();
            GetColumn_EmptyColumn_EmptyResponse();
            GetColumn_InvalidBoard_ErrorMessage();
            GetColumn_InvalidColumn_ErrorMessage();
            GetColumn_ProperUse_ResponseWithTasks();
            GetColumnName_AllColumns_ThreeColumnNames();
            Console.WriteLine("");
        }
        /// <summary>
        /// This function tests Requirement 9
        /// </summary>
        public void CreateBoard_ProperUse_EmptyResponse()
        {
            string funcName = "CreateBoard_ProperUse_EmptyResponse";
            string response1 = boardService.CreateBoard(email, funcName + "1");
            string response2 = boardService.CreateBoard(email, funcName + "2");
            string response3 = boardService.CreateBoard(email, funcName + "3");
            Assert.IsEmptyResponse(response1, funcName);
            Assert.IsEmptyResponse(response2, funcName);
            Assert.IsEmptyResponse(response3, funcName);
        }
        /// <summary>
        /// This function tests Requirements 9
        /// </summary>
        public void CreateBoard_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "CreateBoard_UserNotLoggedIn_ErrorMessage";
            userService.Logout(email);
            string response = boardService.CreateBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);
            userService.Login(email, password);
        }
        /// <summary>
        /// This function tests Requirement 12
        /// </summary>
        public void CreateBoard_LimitlessInit_MinusOne()
        {
            string funcName = "CreateBoard_LimitlessInit_MinusOne";
            boardService.CreateBoard(email, funcName);
            string response = boardService.GetColumnLimit(email, funcName, 0);
            Assert.IsReturnEqualTo(response, "-1", funcName);
        }
        /// <summary>
        /// This function tests Requirement 6
        /// </summary>
        public void CreateBoard_BoardAlreadyExists_ErrorMesssage()
        {
            string funcName = "CreateBoard_BoardAlreadyExists_ErrorMesssage";
            boardService.CreateBoard(email, funcName);
            string response = boardService.CreateBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);
        }  
        /// <summary>
        /// This function tests Requirement 6
        /// </summary>
        public void CreateBoard_SameNameDifferentAccounts_EmptyResponse()
        {
            string funcName = "CreateBoard_SameNameDifferentAccounts_EmptyResponse";
            string email2 = funcName + "@gmail.com";
            string response1 = boardService.CreateBoard(email, funcName);
            userService.Register(email2, "Aa1234567");
            string response2 = boardService.CreateBoard(email2, funcName);
            Assert.IsEmptyResponse(response1, funcName);
            Assert.IsEmptyResponse(response2, funcName);
        }
        /// <summary>
        /// This function tests Requirement 9
        /// </summary>
        public void DeleteBoard_ProperUse_EmptyResponse()
        {
            string funcName = "DeleteBoard_ProperUse_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response = boardService.DeleteBoard(email, funcName);
            Assert.IsEmptyResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirements 9
        /// </summary>
        public void DeleteBoard_UserNotLoggedIn_ErrorMessage()
        {
            string funcName = "DeleteBoard_UserNotLoggedIn_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            userService.Logout(email);
            string response = boardService.DeleteBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);
            userService.Login(email, password);

        }
        /// <summary>
        /// This function tests Requirements 9
        /// </summary>
        public void DeleteBoard_BoardDoesNotExist_ErrorMesssage()
        {
            string funcName = "DeleteBoard_BoardDoesNotExist_ErrorMesssage";
            string response = boardService.DeleteBoard(email, funcName);
            Assert.IsErrorMessageResponse(response, funcName);

        }
        /// <summary>
        /// This function tests Requirement 11
        /// </summary>
        public void LimitColumn_ProperUse_EmptyResponse()
        {
            string funcName = "LimitColumn_ProperUse_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName, 0, 2);
            string response2 = taskService.AddTask(email, funcName , "title", "description", DateTime.Now);
            string response3 = taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            Assert.IsEmptyResponse(response1, funcName);
            Assert.IsEmptyResponse(response2, funcName);
            Assert.IsEmptyResponse(response3, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void LimitColumn_InvalidNumber_ErrorMessage()
        { 
            string funcName = "LimitColumn_InvalidNumber_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName, 0,-2);
            Assert.IsErrorMessageResponse(response1, funcName);
        }
        /// <summary>
        /// This function tests Requirement 11
        /// </summary>
        public void LimitColumn_Limitless_EmptyResponse()
        {
            string funcName = "LimitColumn_Limitless_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName , 0, -1);
            Assert.IsEmptyResponse(response1, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void LimitColumn_InvalidColumnOrdinal_ErrorMessage()
        {
            string funcName = "LimitColumn_InvalidColumnOrdinal_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.LimitColumn(email, funcName, -1, 5);
            string response2 = boardService.LimitColumn(email, funcName, 3, 5);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11 & 12
        /// </summary>
        public void GetColumnLimit_ProperUse_Limit()
        {
            string funcName = "GetColumnLimit_ProperUse_Limit";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumnLimit(email, funcName, 0);
            string response2 = boardService.GetColumnLimit(email, funcName, 1);
            string response3 = boardService.GetColumnLimit(email, funcName, 2);
            boardService.LimitColumn(email, funcName, 0, 5);
            boardService.LimitColumn(email, funcName, 1, 5);
            boardService.LimitColumn(email, funcName, 2, 5);
            string response4 = boardService.GetColumnLimit(email, funcName, 0);
            string response5 = boardService.GetColumnLimit(email, funcName, 1);
            string response6 = boardService.GetColumnLimit(email, funcName, 2);
            Assert.IsReturnEqualTo(response1, "-1", funcName);
            Assert.IsReturnEqualTo(response2, "-1", funcName);
            Assert.IsReturnEqualTo(response3, "-1", funcName);
            Assert.IsReturnEqualTo(response4, "5", funcName);
            Assert.IsReturnEqualTo(response5, "5", funcName);
            Assert.IsReturnEqualTo(response6, "5", funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void GetColumnLimit_InvalidColumnOrdinal_ErrorMessage()
        {
            string funcName = "GetColumnLimit_InvalidColumnOrdinal_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumnLimit(email, funcName, -1);
            string response2 = boardService.GetColumnLimit(email, funcName, 3);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);
        }
        /// <summary>
        /// This function tests Requirements 11
        /// </summary>
        public void GetColumnLimit_InvalidBoard_ErrorMessage()
        {
            string funcName = "GetColumnLimit_InvalidBoard_ErrorMessage";
            string response = boardService.GetColumnLimit(email, funcName, 0);
            Assert.IsErrorMessageResponse(response , funcName);
        }
        /// <summary>
        /// This function tests Requirements 14
        /// </summary>
        public void GetColumn_ProperUse_ResponseWithTasks()
        {
            string funcName = "GetColumn_ProperUse_ResponseWithTasks";
            boardService.CreateBoard(email, funcName);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            taskService.AddTask(email, funcName, "title", "description", DateTime.Now);
            try
            {
                string response1 = boardService.GetColumn(email, funcName, 0);
                Response? r1 = JsonSerializer.Deserialize<Response> (response1);
                TaskToSend[] tasks1 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r1.ReturnValue);
                if (tasks1.Length == 3)
                {
                    int task1 = tasks1[0].Id;
                    int task2 = tasks1[1].Id;
                    int task3 = tasks1[2].Id;
                    taskService.AdvanceTask(email, funcName, 0, task1);
                    taskService.AdvanceTask(email, funcName, 1, task1);
                    taskService.AdvanceTask(email, funcName, 0, task2);
                    string response2 = boardService.GetColumn(email, funcName, 0);
                    string response3 = boardService.GetColumn(email, funcName, 1);
                    string response4 = boardService.GetColumn(email, funcName, 2);
                    Response? r2 = JsonSerializer.Deserialize<Response>(response2);
                    Response? r3 = JsonSerializer.Deserialize<Response>(response3);
                    Response? r4 = JsonSerializer.Deserialize<Response>(response4);
                    TaskToSend[] tasks2 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r2.ReturnValue);
                    TaskToSend[] tasks3 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r3.ReturnValue);
                    TaskToSend[] tasks4 = JsonSerializer.Deserialize<TaskToSend[]>((JsonElement)r4.ReturnValue);
                    if (tasks2.Length == 1 && tasks3.Length == 1 && tasks4.Length == 1)
                    {
                        return;
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
        /// This function tests Requirements 5
        /// </summary>
        public void GetColumn_EmptyColumn_EmptyResponse()
        {
            string funcName = "GetColumn_EmptyColumn_EmptyResponse";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumn(email, funcName, 0);
            string response2 = boardService.GetColumn(email, funcName, 0);
            string response3 = boardService.GetColumn(email, funcName, 0);
            Assert.IsReturnValueEmptyArray(response1, funcName);
            Assert.IsReturnValueEmptyArray(response2, funcName);
            Assert.IsReturnValueEmptyArray(response3, funcName);
        }
        /// <summary>
        /// This function tests Requirements 5
        /// </summary>
        public void GetColumn_InvalidColumn_ErrorMessage()
        {
            string funcName = "GetColumn_InvalidColumn_ErrorMessage";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumn(email, funcName, -1);
            string response2 = boardService.GetColumn(email, funcName, 3);
            Assert.IsErrorMessageResponse(response1, funcName);
            Assert.IsErrorMessageResponse(response2, funcName);

        }
        /// <summary>
        /// This function tests Requirements 5
        /// </summary>
        public void GetColumn_InvalidBoard_ErrorMessage()
        {
            string funcName = "GetColumn_InvalidBoard_ErrorMessage";
            string response = boardService.GetColumn(email, funcName, 0);
            Assert.IsErrorMessageResponse(response, funcName);
        }
        /// <summary>
        /// This function tests Requirement 5
        /// </summary>
        public void GetColumnName_AllColumns_ThreeColumnNames()
        {
            string funcName = "GetColumnName_AllColumns_ThreeColumnNames";
            boardService.CreateBoard(email, funcName);
            string response1 = boardService.GetColumnName(email, funcName, 0);
            string response2 = boardService.GetColumnName(email, funcName, 1);
            string response3 = boardService.GetColumnName(email, funcName, 2);
            Assert.IsReturnEqualTo(response1, COLUMNS_NAMES[0], funcName);
            Assert.IsReturnEqualTo(response2, COLUMNS_NAMES[1], funcName);
            Assert.IsReturnEqualTo(response3, COLUMNS_NAMES[2], funcName);
        }
    }
}
