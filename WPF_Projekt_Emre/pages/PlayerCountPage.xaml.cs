using System.Windows.Controls;
using WPF_Projekt_Emre.ViewModels;

namespace WPF_Projekt_Emre.pages
{
    public partial class PlayerCountPage : Page
    {
        public PlayerCountPage(Frame mainFrame)
        {
            InitializeComponent();

            DataContext = new PlayerCountViewModel(mainFrame);
        }
    }
}