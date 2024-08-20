// See https://aka.ms/new-console-template for more information
using BackendTests;
using System.Text.Json;
using System.Text.Json.Nodes;
using IntroSE.Kanban.Backend.ServiceLayer;

WrapperService wrapperService = new WrapperService();
UserServiceTests userServiceTests = new UserServiceTests(wrapperService);
BoardServiceTests boardServiceTests = new BoardServiceTests(wrapperService);
TaskServiceTests taskServiceTests = new TaskServiceTests(wrapperService);
userServiceTests.RunTests();
boardServiceTests.RunTests();
taskServiceTests.RunTests();
