using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSc : MonoBehaviour
{
    /*[EnumPaging]
    public CollectableType type;
    public enum CollectableType
    {
        Juice, Vacuum, Tank
    }

    private bool IsJuice() => type == CollectableType.Juice;
    private bool IsVacuum() => type == CollectableType.Vacuum;
    private bool IsTank() => type == CollectableType.Tank;*/

    public bool increase = true;
    public int level = 1;
    public GameObject takeAnim;
    public Color takeSplashColor;
    public Vector3 rotateDirection = Vector3.zero;
<<<<<<< HEAD
    public bool isJuice, isVacuum, isTank;
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d

    private int effectFactor;
    private GameObject player;
    private GameManager gameManager;
    private bool triged = false;
    public float timer = 0;
    private bool collected = false;
    private Vector3 movementUpperPoint;
<<<<<<< HEAD
    private Vector3 startPos;
    private float snakeSpeed, snakeXMovement;
    private bool isRight = true;
=======

>>>>>>> e135bd62164667161091742e0478e6084b9b368d
    private void Awake()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        effectFactor = increase ? level : -level;
        movementUpperPoint = transform.localPosition + Vector3.up * 0.5f;
    }

    private void FixedUpdate()
    {
        if (!collected)
        {
            transform.Rotate(rotateDirection * gameManager.rotateObjectsSens * Time.deltaTime);
<<<<<<< HEAD
            /*if(!IsJuice())
=======
            if(!IsJuice())
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            {
                timer += Time.deltaTime;
                if (timer <= 1)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, movementUpperPoint, gameManager.swingObjectsSens * Time.deltaTime);
                }
                else if (timer <= 2)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, movementUpperPoint - Vector3.up, gameManager.swingObjectsSens * Time.deltaTime);
                }
                else
                {
                    timer = 0;
                }
<<<<<<< HEAD
            }*/
            /*timer += Time.deltaTime;
            if (timer <= 1)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, movementUpperPoint, gameManager.swingObjectsSens * Time.deltaTime);
            }
            else if (timer <= 2)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, movementUpperPoint - Vector3.up, gameManager.swingObjectsSens * Time.deltaTime);
            }
            else
            {
                timer = 0; 
            }*/
        }
    }

    public void SnakeMovement(float speed, float x, float delay)
    {
        snakeSpeed = speed;
        snakeXMovement = x;
        startPos = transform.localPosition;
        InvokeRepeating("SnakeMovementLoop", delay, Time.fixedDeltaTime);
    }

    public void SnakeMovementLoop()
    {
        if (isRight)
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = Vector3.MoveTowards(pos, startPos + Vector3.right * snakeXMovement, snakeSpeed * Time.fixedDeltaTime);
            if (transform.localPosition == startPos + Vector3.right * snakeXMovement)
            {
                isRight = false;
            }
        }
        else
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = Vector3.MoveTowards(pos, startPos - Vector3.right * snakeXMovement, snakeSpeed * Time.fixedDeltaTime);
            if (transform.localPosition == startPos - Vector3.right * snakeXMovement)
            {
                isRight = true;
=======
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("VacuumArea") && !collected)
        {
            gameManager.audioManager.Play("VacuumObj");
            gameManager.Vibrate();
            collected = true;
            Destroy(Instantiate(takeAnim, transform.position, Quaternion.identity), 1f);
            transform.parent = player.transform.Find("Collecteds");
            InvokeRepeating("MoveToPlayer", 0, Time.fixedDeltaTime);
        }
    }

    private void MoveToPlayer()
    {
        transform.Find("Obj").localPosition = Vector3.MoveTowards(transform.Find("Obj").localPosition, Vector3.zero, gameManager.collectSens * 2 * Time.fixedDeltaTime);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, gameManager.collectSens * Time.fixedDeltaTime);
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, (gameManager.collectSens * 2 / 3) * Time.fixedDeltaTime);
        

<<<<<<< HEAD
        if (isJuice)
        {
            //transform.Find("Obj").LookAt(transform.parent.position);
            transform.LookAt(transform.parent.position);

            float key = transform.Find("Obj").GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) + gameManager.shapeSens * Time.fixedDeltaTime;
            key = Mathf.Clamp(key, 0, 75);
            transform.Find("Obj").GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, key);
        }
        
        /*if (IsJuice())
        {
            //transform.Find("Obj").LookAt(transform.parent.position);
            transform.LookAt(transform.parent.position);

            float key = transform.Find("Obj").GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) + gameManager.shapeSens * Time.fixedDeltaTime;
            key= Mathf.Clamp(key,0,75);
            transform.Find("Obj").GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, key);
        }*/

        if (transform.localPosition == Vector3.zero && !triged)
        {
            transform.localScale = Vector3.zero;
            triged = true;

            if (isJuice)
            {
                GameObject.Find("VacuumPipe").transform.Find("left").GetComponent<LineConnector>().PipeGetAnimTrigger(gameObject.GetComponent<CollectableSc>());
            }
            else if(isVacuum)
            {
                gameManager.SetVacuum(effectFactor);
                Destroy(gameObject);
=======
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
            else if (IsTank())
            {
                gameManager.SetTankCapacity(effectFactor);
                Destroy(gameObject);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d
            }
            else if(isTank)
            {
                gameManager.SetTankCapacity(effectFactor);
                Destroy(gameObject);
            } 

            /*if (IsJuice())
            {
                GameObject.Find("VacuumPipe").transform.Find("left").GetComponent<LineConnector>().PipeGetAnimTrigger(gameObject.GetComponent<CollectableSc>());
            }
            else if (IsVacuum())
            {
                gameManager.SetVacuum(effectFactor);
                Destroy(gameObject);
            }
            else if (IsTank())
            {
                gameManager.SetTankCapacity(effectFactor);
                Destroy(gameObject);
            }*/

            CancelInvoke("MoveToPlayer"); 
        }
    }

    public void TakeTheFruit()
    {
<<<<<<< HEAD
        if (increase)
        {
            GameObject getEffect = Instantiate(gameManager.getJuiceParticle, gameManager.tankShader.transform.position, Quaternion.identity);
            getEffect.GetComponent<ParticleSystem>().startColor = takeSplashColor;
            getEffect.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = takeSplashColor;
            getEffect.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = takeSplashColor;
            gameManager.ChangeLiquidColor(takeSplashColor);
            Destroy(getEffect, 1f);
        }
        else
        {
            GameObject getEffect = Instantiate(gameManager.getPoisonParticle, gameManager.tankShader.transform.position, Quaternion.identity, gameManager.tankShader.transform.parent);
            Destroy(getEffect, 1f);
        }
=======
        GameObject getEffect = Instantiate(gameManager.getJuiceParticle, gameManager.tankShader.transform.position, Quaternion.identity);
        getEffect.GetComponent<ParticleSystem>().startColor = takeSplashColor;
        getEffect.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = takeSplashColor;
        getEffect.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = takeSplashColor;
        gameManager.ChangeLiquidColor(takeSplashColor);
        Destroy(getEffect, 1f);
>>>>>>> e135bd62164667161091742e0478e6084b9b368d

        gameManager.FillTank(effectFactor);
        Destroy(gameObject);
    }
}
