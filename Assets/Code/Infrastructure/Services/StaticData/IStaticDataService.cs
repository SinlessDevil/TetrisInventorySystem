using Code.Infrastructure.StaticData;

namespace Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        void LoadData();
        BalanceStaticData Balance { get; }
    }
}