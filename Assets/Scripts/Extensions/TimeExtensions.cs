using UnityEngine;

namespace Extensions
{
    public static class TimeExtensions
    {
        public static float TimeSinceStart => Time.time;

        public static float DeltaTime => Time.deltaTime;

        public static float ScaledDeltaTime => Time.deltaTime * Time.timeScale;

        public static System.Collections.IEnumerator Wait(this float duration)
        {
            yield return new WaitForSeconds(duration);
        }
    }
}