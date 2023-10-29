using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGate : MonoBehaviour
{
    public GameObject takeAnim;
    public bool increase = true;
    public int level = 1;
    public float speed = 5;
    public bool isMoving = false;

    public bool isRight = true;
    private GameManager gameManager;
    private int effectFactor;
    private bool collected = false;
    private Vector3 startPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        effectFactor = increase ? level : -level;
        if(isMoving)
        {
            startPos = transform.localPosition;
            InvokeRepeating("MoveGate", 0, Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VacuumArea") && !collected)
        {
            gameManager.audioManager.Play("VacuumObj");
            gameManager.Vibrate();
            collected = true;
            Destroy(Instantiate(takeAnim, transform.position + Vector3.up * 4, Quaternion.identity), 1f);
            gameManager.SetTankCapacity(effectFactor);

            Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up*2, 5, gameManager.gatesLayerMask);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Gate")) 
                {
                    collider.enabled = false;
                }
            }

            Destroy(gameObject);
        }
    }

    void MoveGate()
    {
        if (isRight)
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = Vector3.MoveTowards(pos, startPos + Vector3.right * 3, speed * Time.fixedDeltaTime);
            if (transform.localPosition == startPos + Vector3.right * 3)
            {
                isRight = false;
            }
        }
        else
        {
            Vector3 pos = transform.localPosition;
            transform.localPosition = Vector3.MoveTowards(pos, startPos - Vector3.right * 3, speed * Time.fixedDeltaTime);
            if (transform.localPosition == startPos - Vector3.right * 3)
            {
                isRight = true;
            }
        }
    }
}
