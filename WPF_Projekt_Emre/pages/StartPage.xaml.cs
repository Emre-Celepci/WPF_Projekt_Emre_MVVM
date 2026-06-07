using System.Windows.Controls;
using WPF_Projekt_Emre.ViewModels;

namespace WPF_Projekt_Emre.pages
{
    public partial class StartPage : Page
    {
        public StartPage(Frame mainFrame)
        {
            InitializeComponent();

            DataContext = new StartViewModel(mainFrame);
        }
    }
}