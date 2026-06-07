using System.ComponentModel.DataAnnotations;

namespace WPF_Projekt_Emre.models
{
    public class Field : GameEntity
    {
        [Key]
        public int FieldID { get; set; }

        public string FieldName { get; set; } = string.Empty;

        public string FieldTyp { get; set; } = string.Empty;

        public override string GetDisplayName()
        {
            return FieldName;
        }
    }
}