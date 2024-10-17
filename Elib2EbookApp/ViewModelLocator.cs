using CommonServiceLocator;
using NLog;
using ServiceProvider = MainServiceProvider.MainServiceProvider;

namespace Elib2EbookApp
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => ServiceProvider.Current);

            ServiceProvider.Current.Register<ILogger>(() => App.Logger);
            ServiceProvider.Current.Register<MainViewModel>();
        }

        public static MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}