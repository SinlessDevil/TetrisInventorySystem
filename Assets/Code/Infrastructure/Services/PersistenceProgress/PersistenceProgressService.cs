using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.PersistenceProgress.Player;
using Services.PersistenceProgress.Player;

namespace Services.PersistenceProgress
{
    public class PersistenceProgressService : IPersistenceProgressService
    {
        public PlayerData PlayerData { get; set; }
    }
}