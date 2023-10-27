using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleLoader : MonoBehaviour
{
    public GameObject idlePrefab;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(idlePrefab);
        Invoke("LoadScene", 2);
    }

    void LoadScene()
    {
    }
}
