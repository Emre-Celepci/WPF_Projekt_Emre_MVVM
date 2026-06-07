using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Projekt_Emre.models;
using WPF_Projekt_Emre.ViewModels;

namespace WPF_Projekt_Emre.pages
{
    public partial class GameBoardPage : Page
    {
        public GameBoardPage(Frame mainFrame, List<Player> players)
        {
            InitializeComponent();

            DataContext = new GameBoardViewModel(mainFrame, players);
        }

        private void DiceImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is GameBoardViewModel viewModel &&
                viewModel.RollDiceCommand.CanExecute(null))
            {
                viewModel.RollDiceCommand.Execute(null);
            }
        }
    }
}