using System.Windows.Controls;
using WPF_Projekt_Emre.ViewModels;

namespace WPF_Projekt_Emre.pages
{
    public partial class PlayerConfigPage : Page
    {
        public PlayerConfigPage(Frame mainFrame, int playerCount)
        {
            InitializeComponent();

            DataContext = new PlayerConfigViewModel(mainFrame, playerCount);
        }
    }
}