using UnityEngine.Events;
using UnityEngine.UI;

namespace Extensions{
    public static class ButtonExtensions{
        public static void AddListener(this Button button, UnityAction callback){
            button.onClick.AddListener(callback);
        }

        public static void RemoveListener(this Button button, UnityAction callback){
            button.onClick.RemoveListener(callback);
        }

        public static void RemoveAllListeners(this Button button){
            button.onClick.RemoveAllListeners();
        }
    }
}