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
    public GameObject takeAnim;
    public Color takeSplashColor;

    private int effectFactor;
    private GameObject player;
    private GameManager gameManager;
    private bool triged = false;

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
            Destroy(Instantiate(takeAnim, transform.position, Quaternion.identity), 1f);
            transform.parent = player.transform.Find("Collecteds");
            InvokeRepeating("MoveToPlayer", 0, Time.fixedDeltaTime);
        }
    }

    private void MoveToPlayer()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, gameManager.collectSens * Time.fixedDeltaTime);
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, (gameManager.collectSens * 2 / 3) * Time.fixedDeltaTime);

        if (transform.localPosition == Vector3.zero && !triged)
        {
            triged = true;
            if (IsJuice())
            {
                GameObject.Find("VacuumPipe").transform.Find("left").GetComponent<LineConnector>().PipeGetAnimTrigger(gameObject.GetComponent<CollectableSc>());
            }
            else if (IsVacuum())
            {
                gameManager.SetVacuum(effectFactor);
                Destroy(gameObject);
            }

            CancelInvoke("MoveToPlayer"); 
        }
    }

    public void TakeTheFruit()
    {
        GameObject getEffect = Instantiate(gameManager.getJuiceParticle, gameManager.tankShader.transform.position, Quaternion.identity);
        getEffect.GetComponent<ParticleSystem>().startColor = takeSplashColor;
        getEffect.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = takeSplashColor;
        getEffect.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = takeSplashColor;
        Destroy(getEffect, 1f);
        gameManager.FillTank(effectFactor);
        Destroy(gameObject);
    }
}
