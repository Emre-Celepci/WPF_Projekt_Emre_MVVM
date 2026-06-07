using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Projekt_Emre.Commands;
using WPF_Projekt_Emre.pages;
using WPF_Projekt_Emre.Services;

namespace WPF_Projekt_Emre.ViewModels
{
    public class HighscoreViewModel : BaseViewModel
    {
        private readonly Frame _mainFrame;
        private readonly HighscoreService _highscoreService;

        public ObservableCollection<HighscoreService.HighscoreView> Highscores { get; } = new();

        public ICommand BackCommand { get; }

        public HighscoreViewModel(Frame mainFrame)
        {
            _mainFrame = mainFrame;
            _highscoreService = new HighscoreService();

            LoadHighscores();

            BackCommand = new RelayCommand(_ =>
            {
                _mainFrame.Navigate(new StartPage(_mainFrame));
            });
        }

        private void LoadHighscores()
        {
            Highscores.Clear();

            foreach (HighscoreService.HighscoreView highscore in _highscoreService.GetTop10Highscores())
            {
                Highscores.Add(highscore);
            }
        }
    }
}