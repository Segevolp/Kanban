using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class WrapperService
    {
        public UserService userService;
        public BoardService boardService;
        public TaskService taskService;
        internal static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WrapperService() 
        {
            userService = new UserService();
            boardService = new BoardService(userService.userFacade);
            taskService = new TaskService(boardService.boardFacade);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.Config"));
            log.Info("Starting log!");
        }

    }
}
