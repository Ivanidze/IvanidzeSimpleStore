namespace Infrasturcture
{
    public interface IViewFactory
    {
        TViewModel ShowView<TViewModel>();
        TViewModel ResolveViewModel<TViewModel>();
    }
}
