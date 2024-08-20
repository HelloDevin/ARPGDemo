using System;
using UnityEngine;

namespace ZZZ
{
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                    //让游戏物体可以在场景切换时不被卸载
                    DontDestroyOnLoad(obj);
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}