using System.Collections.Generic;

namespace Services.Navigation.Runtime.Scripts
{
    public interface IRoute
    {
        IReadOnlyList<IHighway> Highways { get; }
    }

    public interface IHighway
    {
        IReadOnlyList<IPoint> Points { get; }
    }
    
}