using StaticData;

namespace Code.Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        void LoadData();
        BalanceStaticData BalanceStaticData { get; }
    }
}