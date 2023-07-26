using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSc : MonoBehaviour
{
    // 0 No resource, 1 Preparing, 2 Ready
    public int status = 0;
    public float prepareDuration = 1;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetStatus(int stat)
    {
        status = stat;
        switch (status)
        {
            case 0:
                // No resource
                break;

            case 1: // Preparing
                timer = 0;
                InvokeRepeating("PrepareProduct", 0, Time.deltaTime);
                
                break;

            case 2:
                // Ready
                break;
        }
    }

    private void PrepareProduct()
    {
        timer += Time.deltaTime;
        if(timer >= prepareDuration && status == 1)
        {
            ProductPrepared();
            CancelInvoke("PrepareProduct");
        }
    }

    private void ProductPrepared()
    {
        SetStatus(2);
    }


}
