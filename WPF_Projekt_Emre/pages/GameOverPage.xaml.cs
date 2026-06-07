using System.Collections.Generic;
using System.Windows.Controls;
using WPF_Projekt_Emre.models;
using WPF_Projekt_Emre.ViewModels;

namespace WPF_Projekt_Emre.pages
{
    public partial class GameOverPage : Page
    {
        public GameOverPage(Frame mainFrame, List<Player> players)
        {
            InitializeComponent();

            DataContext = new GameOverViewModel(mainFrame, players);
        }
    }
}