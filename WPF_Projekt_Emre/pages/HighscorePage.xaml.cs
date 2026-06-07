using System.Windows.Controls;
using WPF_Projekt_Emre.ViewModels;

namespace WPF_Projekt_Emre.pages
{
    public partial class HighscorePage : Page
    {
        public HighscorePage(Frame mainFrame)
        {
            InitializeComponent();

            DataContext = new HighscoreViewModel(mainFrame);
        }
    }
}