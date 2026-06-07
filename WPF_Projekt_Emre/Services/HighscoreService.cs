using System.Collections.Generic;
using System.Linq;
using WPF_Projekt_Emre.data;
using WPF_Projekt_Emre.models;

namespace WPF_Projekt_Emre.Services
{
    public class HighscoreService
    {
        public class HighscoreView
        {
            public int Platz { get; set; }
            public string PlayerName { get; set; } = string.Empty;
            public int Score { get; set; }
        }

        public void SaveResults(List<Player> players)
        {
            using GameDbContext db = new GameDbContext();

            foreach (Player player in players)
            {
                Player? existingPlayer = db.Players
                    .FirstOrDefault(p => p.PlayerName == player.PlayerName);

                if (existingPlayer == null)
                {
                    existingPlayer = new Player
                    {
                        PlayerName = player.PlayerName,
                        Figur = player.Figur,
                        Score = player.Score
                    };

                    db.Players.Add(existingPlayer);
                    db.SaveChanges();
                }
                else
                {
                    existingPlayer.Figur = player.Figur;
                    existingPlayer.Score = player.Score;
                }

                Highscore? existingHighscore = db.Highscores
                    .FirstOrDefault(h => h.PlayerID == existingPlayer.PlayerID);

                if (existingHighscore == null)
                {
                    Highscore newHighscore = new Highscore
                    {
                        PlayerID = existingPlayer.PlayerID,
                        PlayerName = existingPlayer.PlayerName,
                        Score = player.Score
                    };

                    db.Highscores.Add(newHighscore);
                }
                else if (player.Score > existingHighscore.Score)
                {
                    existingHighscore.Score = player.Score;
                    existingHighscore.PlayerName = existingPlayer.PlayerName;
                }
            }

            db.SaveChanges();
        }

        public List<HighscoreView> GetTop10Highscores()
        {
            using GameDbContext db = new GameDbContext();

            List<Highscore> highscores = db.Highscores
                .OrderByDescending(h => h.Score)
                .Take(10)
                .ToList();

            List<HighscoreView> result = new List<HighscoreView>();

            for (int i = 0; i < highscores.Count; i++)
            {
                result.Add(new HighscoreView
                {
                    Platz = i + 1,
                    PlayerName = highscores[i].PlayerName,
                    Score = highscores[i].Score
                });
            }

            return result;
        }
    }
}