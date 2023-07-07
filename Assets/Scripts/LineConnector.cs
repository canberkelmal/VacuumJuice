using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public float animSens = 1f;
    public GameObject[] _objs;

    private LineRenderer line;
    private int currentKey = 1;
    private CollectableSc currentCollectable;

    private void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        for(int i=0; i < _objs.Length; i++)
        {
            //Debug.Log(line.positionCount);
            line.SetPosition(i, _objs[i].transform.position + Vector3.up * 0.06f);
        }
    }

    public void PipeGetAnimTrigger(CollectableSc sc)
    {
        currentCollectable = sc;
        InvokeRepeating("PipeGetAnim", 0, Time.fixedDeltaTime);
    }

    private void PipeGetAnim()
    {
        if ( currentKey == 1)
        {
            _objs[currentKey].transform.localScale += Vector3.one * animSens * Time.deltaTime;
            _objs[currentKey + 1].transform.localScale += Vector3.one * animSens * Time.deltaTime / 2;
            if (_objs[currentKey].transform.localScale.x >= 0.5)
            {
                _objs[currentKey].transform.localScale = Vector3.one * 0.5f;
                _objs[currentKey + 1].transform.localScale = Vector3.one * 0.3f;
                currentKey++;
            }
        }
        else if (currentKey > 1 && currentKey < _objs.Length-2)
        {
            _objs[currentKey - 1].transform.localScale -= Vector3.one * animSens * Time.deltaTime;
            _objs[currentKey].transform.localScale += Vector3.one * animSens * Time.deltaTime;
            _objs[currentKey + 1].transform.localScale += Vector3.one * animSens * Time.deltaTime / 2;
            if (_objs[currentKey].transform.localScale.x >= 0.5)
            {
                _objs[currentKey - 1].transform.localScale = Vector3.one * 0.1f;
                _objs[currentKey].transform.localScale = Vector3.one * 0.5f;
                _objs[currentKey + 1].transform.localScale = Vector3.one * 0.3f;
                currentKey++;
            }

        }
        else if (currentKey == _objs.Length - 2 && _objs[currentKey-1].transform.localScale.x > 0.1f)
        {
            _objs[currentKey - 1].transform.localScale -= Vector3.one * animSens * Time.deltaTime;
            _objs[currentKey].transform.localScale += Vector3.one * animSens * Time.deltaTime;
        }
        else if (currentKey == _objs.Length - 2 && _objs[currentKey - 1].transform.localScale.x <= 0.1f)
        {
            _objs[currentKey].transform.localScale -= Vector3.one * animSens * Time.deltaTime;
            if (_objs[currentKey].transform.localScale.x <= 0.1f)
            {
                _objs[currentKey - 1].transform.localScale = Vector3.one * 0.1f;
                _objs[currentKey].transform.localScale = Vector3.one * 0.1f;
                currentKey = 1;
                currentCollectable.TakeTheFruit();
                CancelInvoke("PipeGetAnim");
            }
        }
    }
    /*private void PipeGetAnim()
    {
        Debug.Log(line.widthCurve.keys[currentKey].value);
        if (currentKey == 0)
        {
            line.widthCurve.keys[currentKey].value = Mathf.MoveTowards(0.25f, 0.75f, animSens * Time.deltaTime);
            if (line.widthCurve.keys[currentKey].value == 0.75)
            {
                currentKey++;
            }
        }
        else if (currentKey > 0 && currentKey < line.widthCurve.keys.Length - 1)
        {
            line.widthCurve.keys[currentKey].value = Mathf.MoveTowards(0.25f, 0.75f, animSens * Time.deltaTime);
            line.widthCurve.keys[currentKey - 1].value = Mathf.MoveTowards(0.75f, 0.25f, animSens * Time.deltaTime);
            if (line.widthCurve.keys[currentKey].value == 0.75)
            {
                currentKey++;
            }

        }
        else if (currentKey == line.widthCurve.keys.Length - 1 && line.widthCurve.keys[currentKey - 1].value > 0.25f)
        {
            line.widthCurve.keys[currentKey].value = Mathf.MoveTowards(0.25f, 0.75f, animSens * Time.deltaTime);
            line.widthCurve.keys[currentKey - 1].value = Mathf.MoveTowards(0.75f, 0.25f, animSens * Time.deltaTime);
        }
        else if (currentKey == line.widthCurve.keys.Length - 1 && line.widthCurve.keys[currentKey - 1].value == 0.25f)
        {
            line.widthCurve.keys[currentKey].value = Mathf.MoveTowards(0.75f, 0.25f, animSens * Time.deltaTime);
            if (line.widthCurve.keys[currentKey].value == 0.25)
            {
                currentKey = 0;
                currentCollectable.TakeTheFruit();
                CancelInvoke("PipeGetAnim");
            }
        }
    }*/

}
