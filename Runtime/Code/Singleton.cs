using UnityEngine;

namespace MyGrid.Code
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = (T) FindObjectOfType(typeof(T));
                return _instance;
            }
        }
    }
}