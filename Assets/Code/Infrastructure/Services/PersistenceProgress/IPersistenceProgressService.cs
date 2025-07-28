using Code.Infrastructure.Services.PersistenceProgress.Player;

namespace Code.Infrastructure.Services.PersistenceProgress
{
    public interface IPersistenceProgressService
    {
        PlayerData PlayerData { get; set; }
    }
}