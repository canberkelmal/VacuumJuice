using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class IdleUpgradesSc : MonoBehaviour
{
    public float upgradeCost = 1f;

    private IdleManager idleManager;


    public void Awake()
    {
        idleManager = GameObject.Find("IdleManager").GetComponent<IdleManager>();

        Button lvButton = transform.Find("Button").GetComponent<Button>();

        lvButton.transform.Find("UpgradeCostTx").GetComponent<TextMeshProUGUI>().text = "<sprite=12>" + idleManager.ConvertNumberToUIText(upgradeCost);

        lvButton.onClick.AddListener(() => idleManager.SetMoneyCount(-upgradeCost));
    }
}
