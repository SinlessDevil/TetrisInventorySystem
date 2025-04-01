using StaticData;
using UnityEngine;

namespace Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string BalanceStaticDataPath = "StaticData/Balance";

        private BalanceStaticData _balanceStaticData;

        public BalanceStaticData BalanceStaticData => _balanceStaticData;

        public void LoadData()
        {
            _balanceStaticData = Resources.Load<BalanceStaticData>(BalanceStaticDataPath);
        }
    }
}