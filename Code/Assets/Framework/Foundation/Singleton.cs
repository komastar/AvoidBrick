using UnityEngine;

namespace Assets.Framework.Foundation
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>, new()
    {
        private static readonly object lockObj = new object();
        private static T instance = null;
        public static T Get()
        {
            lock (lockObj)
            {
                if (null == instance)
                {
                    T[] objs = FindObjectsOfType<T>();

                    if (0 < objs.Length)
                    {
                        instance = objs[0];
                    }

                    if (1 < objs.Length)
                    {
                        Log.Error("There is more than one " + typeof(T).Name + " in the scene.");
                    }

                    if (true == ReferenceEquals(null, instance))
                    {
                        string goName = typeof(T).ToString();
                        GameObject go = GameObject.Find(goName);
                        if (go == null)
                        {
                            go = new GameObject(goName);
                        }
                        instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }
                }

                return instance;
            }
        }
    }
}
