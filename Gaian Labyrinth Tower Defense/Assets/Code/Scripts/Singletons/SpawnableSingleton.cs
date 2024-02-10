using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SpawnableSingleton<T> : MonoBehaviour
        where T : SpawnableSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var instanceGameObject = new GameObject(typeof(T).Name, typeof(T));
                    DontDestroyOnLoad(instanceGameObject);
                    instance = instanceGameObject.GetComponent<T>();
                }

                return instance;
            }
        }
    }