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
<<<<<<< HEAD
        Vector2 targetPos = transform.parent.Find("Icon").localPosition;
        transform.localPosition = Vector2.Lerp(transform.localPosition, targetPos, gM.getCupSens * Time.deltaTime);
        if(transform.localPosition.y < targetPos.y + 15 && transform.localPosition.y > targetPos.y - 15 && !coled)
=======
        transform.localPosition = Vector2.Lerp(transform.localPosition, Vector2.zero, gM.getCupSens * Time.deltaTime);
        if(transform.localPosition.y < 15 && transform.localPosition.y > -15 && !coled)
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        {
            coled = true;
            gM.IncreaseCupCount();
            Destroy(gameObject);
        }
    }
}
