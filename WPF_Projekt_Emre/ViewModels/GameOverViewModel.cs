using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Projekt_Emre.Commands;
using WPF_Projekt_Emre.models;
using WPF_Projekt_Emre.pages;
using WPF_Projekt_Emre.Services;

namespace WPF_Projekt_Emre.ViewModels
{
    public class GameOverViewModel : BaseViewModel
    {
        private readonly Frame _mainFrame;
        private readonly HighscoreService _highscoreService;

        public ObservableCollection<PlayerResultViewModel> PlayerResults { get; } = new();

        public ICommand BackToMenuCommand { get; }

        public string WinnerText { get; private set; } = string.Empty;

        public GameOverViewModel(Frame mainFrame, List<Player> players)
        {
            _mainFrame = mainFrame;
            _highscoreService = new HighscoreService();

            CreatePlayerResults(players);
            SaveHighscores(players);

            BackToMenuCommand = new RelayCommand(_ =>
            {
                _mainFrame.Navigate(new StartPage(_mainFrame));
            });
        }

        private void CreatePlayerResults(List<Player> players)
        {
            List<Player> sortedPlayers = players
                .OrderByDescending(p => p.Score)
                .ToList();

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                Player player = sortedPlayers[i];

                PlayerResults.Add(new PlayerResultViewModel
                {
                    Platz = i + 1,
                    PlayerName = player.PlayerName,
                    Score = player.Score
                });
            }

            if (PlayerResults.Count > 0)
            {
                PlayerResultViewModel winner = PlayerResults.First();
                WinnerText = $"🎉 Gewinner: {winner.PlayerName} mit {winner.Score} Punkten!";
            }
            else
            {
                WinnerText = "Kein Spielergebnis vorhanden.";
            }

            OnPropertyChanged(nameof(WinnerText));
        }

        private void SaveHighscores(List<Player> players)
        {
            try
            {
                _highscoreService.SaveResults(players);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Fehler beim Speichern der Spielergebnisse:\n\n" + ex.Message,
                    "Datenbankfehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }

    public class PlayerResultViewModel
    {
        public int Platz { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int Score { get; set; }
    }
}