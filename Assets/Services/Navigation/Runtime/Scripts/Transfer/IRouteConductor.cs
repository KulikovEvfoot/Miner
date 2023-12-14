namespace Services.Navigation.Runtime.Scripts.Transfer
{
    public interface IRouteConductor
    {
        RouteConductorResult Conduct(RouteConductorArgs args);
    }
}