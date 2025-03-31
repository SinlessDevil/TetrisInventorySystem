using Services.PersistenceProgress.Player;

namespace Services.PersistenceProgress
{
    public interface IPersistenceProgressService
    {
        PlayerData PlayerData { get; set; }
    }
}