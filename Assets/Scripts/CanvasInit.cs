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
<<<<<<< HEAD

            /*foreach(Transform child in transform.GetChild(i))
            {
                transform.gameObject.SetActive(false);
            }*/
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
        }
    }
}
