using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSc : MonoBehaviour
{
    public float firstYPos = 400f;
    public float distance = 150f;
    public float animSens = 1;

    private int current = 0;
    private bool animDone = false;

    private IdleManager idleManager;

    private void Start()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();
    }
    public void CloseUpgrade(GameObject upgrade)
    {
        upgrade.SetActive(false);
        PlayerPrefs.SetInt(upgrade.name, 0);
    }
    public void OrderUpgrades()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                Vector2 pos = t.localPosition;
                pos.y = firstYPos - distance * current;
                t.localPosition = pos;
                current++;
            }
        }
    }

    public void Reorder()
    {
        idleManager.CheckForNextLevel();
        InvokeRepeating("ReorderAnim", 0, Time.fixedDeltaTime);
    }
    public void ReorderAnim()
    {
        current = 0;
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                Vector2 pos = t.localPosition;
                pos.y = Mathf.MoveTowards(pos.y, firstYPos - distance * current, animSens*Time.fixedDeltaTime);
                t.localPosition = pos;
                current++;
            }
        }
        animDone = true;
        current = 0;
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                float posY = t.localPosition.y;
                float targetY = firstYPos - distance * current;
                current++;

                if(posY != targetY)
                {
                    animDone = false;
                    break;
                }
            }
        }
        if (animDone)
        {
            CancelInvoke("ReorderAnim");
        }
    }
}
