using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class CollectableSc : MonoBehaviour
{
    [EnumPaging]
    public CollectableType type;
    public enum CollectableType
    {
        Juice, Vacuum, Tank
    }

    private bool IsJuice() => type == CollectableType.Juice;
    private bool IsVacuum() => type == CollectableType.Vacuum;
    private bool IsTank() => type == CollectableType.Tank;

    public bool increase = true;
    public int level = 1;

    private int effectFactor = 1;
    private GameObject player;
    private GameManager gameManager;

    private void Awake()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        effectFactor = increase ? level : -level;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VacuumArea"))
        {
            transform.parent = player.transform.Find("Collecteds");
            InvokeRepeating("MoveToPlayer", 0, Time.fixedDeltaTime);
        }
    }

    private void MoveToPlayer()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, gameManager.collectSens * Time.fixedDeltaTime);
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, (gameManager.collectSens * 2 / 3) * Time.fixedDeltaTime);

        if (transform.localPosition == Vector3.zero) {
            if(IsJuice())
            {
                gameManager.FillTank(effectFactor);
            }
            if (IsVacuum())
            {
                gameManager.SetVacuum(effectFactor);
            }

            CancelInvoke("MoveToPlayer"); 
            Destroy(gameObject);
        }
    }
}
