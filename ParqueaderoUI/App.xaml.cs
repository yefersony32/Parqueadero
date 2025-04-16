using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ParqueaderoUI.Viewss; 
namespace ParqueaderoUI
{
    public partial class App : Application
    {
        private Window m_window = null!;

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(HomePage)); 
            m_window.Content = rootFrame;
            m_window.Activate();
        }
    }
}


