using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaBot.Console.Task;

namespace InstaBot.Console
{
    public interface IInstaBot
    {
        void Run();
    }
    public class InstaBot : IInstaBot
    {
        private ILogin _loginTask { get; set; }

        public InstaBot(ConfigurationManager configurationManager, ILogin loginTask)
        {
            _loginTask = loginTask;
        }

        public void Run()
        {
            _loginTask.DoLogin();
            System.Console.ReadLine();
        }
    }
}
