using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashAnimUI : MonoBehaviour
{
    int price = 0;
    public float moveDur = 1.5f;
    public float moveSens = 1f;
    public float fadeOutDur = 0.5f;
    public float moveUpAmount = 50f;

    float timer = 0;

    public void SpawnCashAnim(float price)
    {
        this.price = (int)price;
        transform.Find("Text").GetComponent<Text>().text = price.ToString() + "$";
        InvokeRepeating("MoneyAnim", 0, Time.fixedDeltaTime);
    }

    public void MoneyAnim()
    {
        timer += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.up, moveSens * Time.deltaTime);
        if(moveDur - timer <= fadeOutDur)
        {
            Color a = transform.Find("Text").GetComponent<Text>().color;
            Color b = transform.Find("Icon").GetComponent<Image>().color;
            a.a = 0;
            b.a = 0;
            transform.Find("Text").GetComponent<Text>().color = Color.Lerp(transform.Find("Text").GetComponent<Text>().color, a, 1 / fadeOutDur * Time.deltaTime);
            transform.Find("Icon").GetComponent<Image>().color = Color.Lerp(transform.Find("Icon").GetComponent<Image>().color, b, 1 / fadeOutDur * Time.deltaTime);
        }
        if(timer >= moveDur)
        {
            Destroy(gameObject);
            CancelInvoke("MoneyAnim");
        }
    }
}
