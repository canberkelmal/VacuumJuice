using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconScript : MonoBehaviour
{
    GameManager gM;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, Vector2.zero, gM.getCupSens * Time.deltaTime);
        if(transform.localPosition.y < 15 && transform.localPosition.y > -15)
        {
            gM.IncreaseCupCount();
            Destroy(gameObject);
        }
    }
}
