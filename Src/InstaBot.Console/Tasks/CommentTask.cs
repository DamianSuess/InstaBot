using System.Threading.Tasks;

namespace InstaBot.Console.Tasks
{
    public interface ICommentTask
    {
        Task Start();
    }
    public class CommentTask : ICommentTask
    {
        public async Task Start()
        {
        }
    }
}