using Services.Navigation.Runtime.Scripts;

namespace Core.Mine.Runtime.RouteBuildType
{
    public interface IRouteBuildStrategy
    {
        IRoute BuildRoute(MineMap mineMap, IRouteBuildType routeBuildType);
    }
}