using System.ComponentModel.DataAnnotations;

namespace WPF_Projekt_Emre.models
{
    public class Figur : GameEntity
    {
        [Key]
        public int FigurID { get; set; }

        public string FigurName { get; set; } = string.Empty;

        public override string GetDisplayName()
        {
            return FigurName;
        }
    }
}