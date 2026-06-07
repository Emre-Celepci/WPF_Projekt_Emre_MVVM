namespace WPF_Projekt_Emre.models
{
    public abstract class GameEntity
    {
        public string ImageName { get; set; } = string.Empty;

        public virtual string GetDisplayName()
        {
            return ImageName;
        }
    }
}