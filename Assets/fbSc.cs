using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fbSc : MonoBehaviour
{
    void Awake()
    {
        if (FB.IsInitialized) // Fb sdk initilaze
            FB.ActivateApp();
        else
            FB.Init(() => { FB.ActivateApp(); });

        //DontDestroyOnLoad(gameObject);
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            if (FB.IsInitialized)
                FB.ActivateApp();
            else
                FB.Init(() => { FB.ActivateApp(); });
        }
    }
}
