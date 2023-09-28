using UnityEngine.SceneManagement;

namespace Extensions
{
    public static class SceneManagerExtensions
    {
        public static int GetSceneNumberFromName()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            for (int i = currentSceneName.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(currentSceneName[i]))
                {
                    return int.Parse(currentSceneName[i].ToString());
                }
            }
            return 0;
        }
    }
}