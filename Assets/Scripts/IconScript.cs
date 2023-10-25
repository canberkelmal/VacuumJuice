using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconScript : MonoBehaviour
{
    GameManager gM;
    bool coled = false;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetPos = transform.parent.Find("Icon").localPosition;
        transform.localPosition = Vector2.Lerp(transform.localPosition, targetPos, gM.getCupSens * Time.deltaTime);
        if(transform.localPosition.y < targetPos.y + 15 && transform.localPosition.y > targetPos.y - 15 && !coled)
        {
            coled = true;
            gM.IncreaseCupCount();
            Destroy(gameObject);
        }
    }
}
