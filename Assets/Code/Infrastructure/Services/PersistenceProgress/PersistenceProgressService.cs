using Code.Infrastructure.Services.PersistenceProgress.Player;

namespace Code.Infrastructure.Services.PersistenceProgress
{
    public class PersistenceProgressService : IPersistenceProgressService
    {
        public PlayerData PlayerData { get; set; }
    }
}