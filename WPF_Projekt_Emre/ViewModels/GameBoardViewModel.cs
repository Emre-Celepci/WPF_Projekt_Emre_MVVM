using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Projekt_Emre.Commands;
using WPF_Projekt_Emre.models;
using WPF_Projekt_Emre.pages;

namespace WPF_Projekt_Emre.ViewModels
{
    public class GameBoardViewModel : BaseViewModel
    {
        private readonly Frame _mainFrame;
        private readonly List<Player> _players;
        private readonly Dictionary<int, Point> _fieldPositions = new();
        private readonly Dictionary<int, string> _fieldTypes = new();
        private readonly Dictionary<Player, int> _playerPositions = new();
        private readonly Dictionary<Player, bool> _skipNextTurn = new();
        private readonly Random _random = new();

        private int _currentPlayerIndex;
        private string _currentPlayerName = string.Empty;
        private int _currentPlayerRound;
        private int _currentPlayerScore;
        private string _diceImageSource = "/assets/Images/Dice/dice6.png";
        private bool _isDiceEnabled = true;

        public ObservableCollection<FieldDisplayViewModel> Fields { get; } = new();
        public ObservableCollection<PlayerFigureViewModel> PlayerFigures { get; } = new();
        public ObservableCollection<PlayerInfoViewModel> PlayerInfos { get; } = new();

        public ICommand RollDiceCommand { get; }

        public string CurrentPlayerName
        {
            get => _currentPlayerName;
            set { _currentPlayerName = value; OnPropertyChanged(); }
        }

        public int CurrentPlayerRound
        {
            get => _currentPlayerRound;
            set { _currentPlayerRound = value; OnPropertyChanged(); }
        }

        public int CurrentPlayerScore
        {
            get => _currentPlayerScore;
            set { _currentPlayerScore = value; OnPropertyChanged(); }
        }

        public string DiceImageSource
        {
            get => _diceImageSource;
            set { _diceImageSource = value; OnPropertyChanged(); }
        }

        public bool IsDiceEnabled
        {
            get => _isDiceEnabled;
            set { _isDiceEnabled = value; OnPropertyChanged(); }
        }

        public GameBoardViewModel(Frame mainFrame, List<Player> players)
        {
            _mainFrame = mainFrame;
            _players = players;

            RollDiceCommand = new RelayCommand(async _ => await RollDiceAsync(), _ => IsDiceEnabled);

            InitFieldPositions();
            InitFieldTypes();
            CreateFieldDisplays();
            InitPlayersOnBoard();
            UpdateCurrentPlayerDisplay();
        }

        private void InitFieldPositions()
        {
            double offsetX = 85;
            double offsetY = 105;
            double step = 65;
            double rightX = offsetX + 5 * step;
            double bottomY = offsetY + 4 * step;

            for (int i = 0; i < 6; i++)
                _fieldPositions[i + 1] = new Point(offsetX + i * step, offsetY);

            for (int i = 0; i < 3; i++)
                _fieldPositions[i + 7] = new Point(rightX, offsetY + (i + 1) * step);

            for (int i = 0; i < 6; i++)
                _fieldPositions[i + 10] = new Point(rightX - i * step, bottomY);

            for (int i = 0; i < 3; i++)
                _fieldPositions[i + 16] = new Point(offsetX, bottomY - (i + 1) * step);
        }

        private void InitFieldTypes()
        {
            for (int i = 1; i <= 18; i++)
                _fieldTypes[i] = "Normal";

            _fieldTypes[1] = "Start";
            _fieldTypes[3] = "DiceAgain";
            _fieldTypes[5] = "ScorePlus";
            _fieldTypes[9] = "RoundSetOut";
            _fieldTypes[11] = "Random";
            _fieldTypes[13] = "ScoreMinus";
            _fieldTypes[17] = "ScorePlus";
        }

        private void CreateFieldDisplays()
        {
            Fields.Clear();

            foreach (KeyValuePair<int, Point> field in _fieldPositions)
            {
                int fieldNumber = field.Key;
                Point position = field.Value;

                string type = _fieldTypes.ContainsKey(fieldNumber)
                    ? _fieldTypes[fieldNumber]
                    : "Normal";

                Fields.Add(new FieldDisplayViewModel
                {
                    Left = position.X,
                    Top = position.Y,
                    ImagePath = $"pack://application:,,,/assets/Images/Layout/Field/{type}Field.png"
                });
            }
        }

        private void InitPlayersOnBoard()
        {
            PlayerFigures.Clear();

            for (int i = 0; i < _players.Count; i++)
            {
                Player player = _players[i];

                player.Runde = 1;
                player.Score = 0;

                _skipNextTurn[player] = false;
                _playerPositions[player] = 1;

                Point start = _fieldPositions[1];

                PlayerFigures.Add(new PlayerFigureViewModel
                {
                    Player = player,
                    Left = start.X + i * 5,
                    Top = start.Y + i * 5,
                    ImagePath = $"pack://application:,,,/assets/Images/Figures/{player.Figur}.png"
                });
            }
        }

        private async Task RollDiceAsync()
        {
            if (!IsDiceEnabled)
                return;

            Player player = _players[_currentPlayerIndex];

            if (_skipNextTurn[player])
            {
                _skipNextTurn[player] = false;
                MessageBox.Show($"{player.PlayerName} setzt diese Runde aus.");

                NextPlayer();
                UpdateCurrentPlayerDisplay();
                return;
            }

            IsDiceEnabled = false;

            for (int i = 0; i < 10; i++)
            {
                int randomValue = _random.Next(1, 7);
                DiceImageSource = $"/assets/Images/Dice/dice{randomValue}.png";
                await Task.Delay(100);
            }

            int steps = _random.Next(1, 7);
            DiceImageSource = $"/assets/Images/Dice/dice{steps}.png";

            await Task.Delay(300);

            MessageBox.Show($"{player.PlayerName} würfelt eine {steps}!");

            MovePlayer(player, steps);

            if (player.Runde > 1)
            {
                MessageBox.Show($"{player.PlayerName} hat das Spiel abgeschlossen!");
                MessageBox.Show("Das Spiel ist beendet!");

                _mainFrame.Navigate(new GameOverPage(_mainFrame, _players));
                return;
            }

            NextPlayer();
            UpdateCurrentPlayerDisplay();

            IsDiceEnabled = true;
        }

        private void MovePlayer(Player player, int steps)
        {
            int currentField = _playerPositions[player];
            int targetField = currentField + steps;
            bool completedRound = false;

            if (targetField > 18)
            {
                targetField %= 18;

                if (targetField == 0)
                    targetField = 18;

                completedRound = true;

                player.Score += 500;
                MessageBox.Show($"{player.PlayerName} hat das Startfeld überquert und erhält 500 Punkte!");
            }

            if (completedRound)
            {
                MessageBox.Show($"{player.PlayerName} hat eine Runde abgeschlossen! ({player.Runde}/5)");
                player.Runde++;
            }

            _playerPositions[player] = targetField;

            int index = _players.IndexOf(player);
            PlayerFigureViewModel figure = PlayerFigures[index];
            Point targetPosition = _fieldPositions[targetField];

            figure.Left = targetPosition.X + index * 5;
            figure.Top = targetPosition.Y + index * 5;

            if (_fieldTypes.TryGetValue(targetField, out string effect))
            {
                ApplyFieldEffect(player, effect);
            }
        }

        private void ApplyFieldEffect(Player player, string effect)
        {
            switch (effect)
            {
                case "DiceAgain":
                    MessageBox.Show($"{player.PlayerName} darf nochmal würfeln!");
                    _currentPlayerIndex--;

                    if (_currentPlayerIndex < 0)
                        _currentPlayerIndex = _players.Count - 1;
                    break;

                case "RoundSetOut":
                    MessageBox.Show($"{player.PlayerName} muss eine Runde aussetzen!");
                    _skipNextTurn[player] = true;
                    break;

                case "ScorePlus":
                    player.Score += 150;
                    MessageBox.Show($"{player.PlayerName} bekommt 150 Punkte!");
                    break;

                case "ScoreMinus":
                    player.Score -= 100;
                    MessageBox.Show($"{player.PlayerName} verliert 100 Punkte!");
                    break;

                case "Random":
                    string[] randomEffects = { "DiceAgain", "RoundSetOut", "ScorePlus", "ScoreMinus" };
                    string chosen = randomEffects[_random.Next(randomEffects.Length)];
                    MessageBox.Show($"{player.PlayerName} erhält einen zufälligen Effekt: {chosen}");
                    ApplyFieldEffect(player, chosen);
                    break;

                case "Normal":
                    player.Score += 50;
                    MessageBox.Show($"{player.PlayerName} bekommt 50 Punkte!");
                    break;
            }

            UpdateCurrentPlayerDisplay();
        }

        private void NextPlayer()
        {
            _currentPlayerIndex++;

            if (_currentPlayerIndex >= _players.Count)
                _currentPlayerIndex = 0;
        }

        private void UpdateCurrentPlayerDisplay()
        {
            Player player = _players[_currentPlayerIndex];

            CurrentPlayerName = player.PlayerName;
            CurrentPlayerRound = player.Runde;
            CurrentPlayerScore = player.Score;

            UpdatePlayerInfoList();
        }

        private void UpdatePlayerInfoList()
        {
            PlayerInfos.Clear();

            for (int i = 0; i < _players.Count; i++)
            {
                Player player = _players[i];
                bool isActive = i == _currentPlayerIndex;

                PlayerInfos.Add(new PlayerInfoViewModel
                {
                    PlayerName = isActive ? "🕹️ " + player.PlayerName : player.PlayerName,
                    Runde = player.Runde,
                    Score = player.Score,
                    IsActive = isActive
                });
            }
        }
    }

    public class FieldDisplayViewModel
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }

    public class PlayerFigureViewModel : BaseViewModel
    {
        private double _left;
        private double _top;

        public Player Player { get; set; } = null!;
        public string ImagePath { get; set; } = string.Empty;

        public double Left
        {
            get => _left;
            set { _left = value; OnPropertyChanged(); }
        }

        public double Top
        {
            get => _top;
            set { _top = value; OnPropertyChanged(); }
        }
    }

    public class PlayerInfoViewModel
    {
        public string PlayerName { get; set; } = string.Empty;
        public int Runde { get; set; }
        public int Score { get; set; }
        public bool IsActive { get; set; }
    }
}