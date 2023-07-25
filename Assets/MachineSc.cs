using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSc : MonoBehaviour
{
    // 0 No fruid, 1 Preparing, 2 Ready
    public int status = 0;
    public float prepareDuration = 1;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStatus(int stat)
    {
        status = stat;
    }


}
