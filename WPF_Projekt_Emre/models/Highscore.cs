using System.ComponentModel.DataAnnotations;

namespace WPF_Projekt_Emre.models
{
    public class Highscore
    {
        [Key]
        public int HighscoreID { get; set; }

        public int PlayerID { get; set; }

        public string PlayerName { get; set; } = string.Empty;

        public int Score { get; set; }
    }
}