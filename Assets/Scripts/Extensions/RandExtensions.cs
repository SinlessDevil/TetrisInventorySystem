using UnityEngine;

namespace Extensions
{
    public static class RandExtensions
    {
        public static bool Chance(this bool value, int percent)
        {
            var rand = Random.Range(0, 11);
            var isChance = rand < percent;
            return isChance;
        }

        public static bool Chance(int percent)
        {
            var rand = Random.Range(0, 11);
            var isChance = rand < percent;
            return isChance;
        }
    }
}