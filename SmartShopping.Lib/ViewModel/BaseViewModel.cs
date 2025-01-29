using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartShopping.Lib.ViewModel;

public abstract class BaseViewModel : ObservableObject, IDisposable
{
    public virtual void Dispose()
    {
    }
}