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

namespace WPF_Projekt_Emre.ViewModels
{
    public class PlayerConfigViewModel : BaseViewModel
    {
        private readonly Frame _mainFrame;
        private readonly int _playerCount;
        private int _currentIndex = 1;

        private string _playerName = string.Empty;
        private FigureOptionViewModel? _selectedFigure;

        public ObservableCollection<FigureOptionViewModel> AvailableFigures { get; } = new();
        public ObservableCollection<PlayerDisplayViewModel> Players { get; } = new();

        public ICommand SelectFigureCommand { get; }
        public ICommand AddPlayerCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand StartGameCommand { get; }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                OnPropertyChanged();
            }
        }

        public string PlayerLabel
        {
            get
            {
                if (Players.Count == _playerCount)
                    return "Alle Spieler wurden hinzugefügt.";

                return $"Spieler {_currentIndex} von {_playerCount}";
            }
        }

        public Visibility AddPlayerButtonVisibility =>
            Players.Count == _playerCount ? Visibility.Collapsed : Visibility.Visible;

        public Visibility StartButtonVisibility =>
            Players.Count == _playerCount ? Visibility.Visible : Visibility.Collapsed;

        public PlayerConfigViewModel(Frame mainFrame, int playerCount)
        {
            _mainFrame = mainFrame;
            _playerCount = playerCount;

            AvailableFigures.Add(new FigureOptionViewModel("Archer"));
            AvailableFigures.Add(new FigureOptionViewModel("Dragon"));
            AvailableFigures.Add(new FigureOptionViewModel("Knight"));
            AvailableFigures.Add(new FigureOptionViewModel("Mage"));

            SelectFigureCommand = new RelayCommand(parameter =>
            {
                if (parameter is not FigureOptionViewModel figure)
                    return;

                foreach (FigureOptionViewModel item in AvailableFigures)
                {
                    item.IsSelected = false;
                }

                figure.IsSelected = true;
                _selectedFigure = figure;
            });

            AddPlayerCommand = new RelayCommand(_ => AddPlayer());

            BackCommand = new RelayCommand(_ =>
            {
                _mainFrame.Navigate(new PlayerCountPage(_mainFrame));
            });

            StartGameCommand = new RelayCommand(_ =>
            {
                if (Players.Count != _playerCount)
                {
                    MessageBox.Show("Bitte füge zuerst alle Spieler hinzu.");
                    return;
                }

                List<Player> players = Players
                    .Select(p => new Player
                    {
                        PlayerName = p.PlayerName,
                        Figur = p.FigurName,
                        Score = 0,
                        Runde = 1
                    })
                    .ToList();

                _mainFrame.Navigate(new GameBoardPage(_mainFrame, players));
            });
        }

        private void AddPlayer()
        {
            string name = PlayerName.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Bitte gib einen Namen ein.");
                return;
            }

            if (_selectedFigure == null)
            {
                MessageBox.Show("Bitte wähle eine Figur.");
                return;
            }

            bool nameAlreadyUsed = Players.Any(p =>
                string.Equals(p.PlayerName, name, StringComparison.OrdinalIgnoreCase));

            if (nameAlreadyUsed)
            {
                MessageBox.Show($"Der Name \"{name}\" wurde in dieser Runde bereits vergeben.");
                return;
            }

            Players.Add(new PlayerDisplayViewModel(
                Players.Count + 1,
                name,
                _selectedFigure.FigureName));

            AvailableFigures.Remove(_selectedFigure);

            PlayerName = string.Empty;
            _selectedFigure = null;

            if (Players.Count < _playerCount)
            {
                _currentIndex++;
            }

            OnPropertyChanged(nameof(PlayerLabel));
            OnPropertyChanged(nameof(AddPlayerButtonVisibility));
            OnPropertyChanged(nameof(StartButtonVisibility));
        }
    }

    public class FigureOptionViewModel : BaseViewModel
    {
        private bool _isSelected;

        public string FigureName { get; }
        public string ImagePath => $"pack://application:,,,/assets/Images/Figures/{FigureName}.png";

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public FigureOptionViewModel(string figureName)
        {
            FigureName = figureName;
        }
    }

    public class PlayerDisplayViewModel
    {
        public int Number { get; }
        public string PlayerName { get; }
        public string FigurName { get; }
        public string ImagePath => $"pack://application:,,,/assets/Images/Figures/{FigurName}.png";

        public PlayerDisplayViewModel(int number, string playerName, string figurName)
        {
            Number = number;
            PlayerName = playerName;
            FigurName = figurName;
        }
    }
}