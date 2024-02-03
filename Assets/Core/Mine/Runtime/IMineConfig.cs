using Core.Mine.Runtime.RouteBuildType;

namespace Core.Mine.Runtime
{
    public interface IMineConfig
    {
        IRouteBuildType GetRouteBuildType();
        MineMap GetMineMap();
    }
}