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
        private ILikeTask _likeTask { get; set; }

        public InstaBot( ILogin loginTask, ILikeTask likeTask)
        {
            _loginTask = loginTask;
            _likeTask = likeTask;
        }

        public void Run()
        {
            _loginTask.DoLogin();
            _likeTask.Start();
            System.Console.ReadLine();
        }
    }
}
