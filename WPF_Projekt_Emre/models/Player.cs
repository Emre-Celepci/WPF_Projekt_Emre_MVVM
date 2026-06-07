using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_Projekt_Emre.models
{
    public class Player
    {
        public int PlayerID { get; set; }          
        public string PlayerName { get; set; }     
        public string Figur { get; set; }          
        public int Score { get; set; }             

        // NICHT in DB!
        [NotMapped] public int Runde { get; set; }
        
    }
}
