using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T instance { get { return _instance; } }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = gameObject.GetComponent<T>();
        }
        else if (_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            Debug.LogWarning(string.Format("Instance of {0} already exists, destroying {1}.", GetType().FullName, ToString()));
        }
    }
}