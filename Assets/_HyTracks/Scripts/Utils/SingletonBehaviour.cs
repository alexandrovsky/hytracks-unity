using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T>:MonoBehaviour where T : SingletonBehaviour<T> {
    public static T instance { get; protected set; }

    public void Awake()
    {
        if(instance != null && instance != this) {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        } else {
            instance = (T)this;
            //DontDestroyOnLoad(instance.gameObject);
        }
        
    }

    
}
