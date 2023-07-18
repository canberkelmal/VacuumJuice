using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(1).GetComponent<Text>().text = (i+1).ToString() + "x";
            transform.GetChild(i).localPosition = transform.GetChild(i - 1).localPosition + Vector3.up;

            Color randomColor = new Color(Random.value, Random.value, Random.value);
            transform.GetChild(i).GetChild(0).GetComponent<Image>().color = randomColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
