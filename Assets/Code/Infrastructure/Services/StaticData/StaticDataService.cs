using Code.Infrastructure.StaticData;
using UnityEngine;

namespace Code.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string BalanceStaticDataPath = "StaticData/Balance/Balance";

        private BalanceStaticData _balanceStaticData;

        public BalanceStaticData Balance => _balanceStaticData;

        public void LoadData()
        {
            Debug.Log("StaticDataService.LoadData");
            _balanceStaticData = Resources.Load<BalanceStaticData>(BalanceStaticDataPath);
            Debug.Log("BalanceStaticData " + _balanceStaticData);
            Debug.Log("BalanceStaticData " + _balanceStaticData.Inventory);
        }
    }
}