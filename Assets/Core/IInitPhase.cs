using System.Threading.Tasks;

namespace Core
{
    public interface IInitPhase
    { 
        Task Init();
    }
}