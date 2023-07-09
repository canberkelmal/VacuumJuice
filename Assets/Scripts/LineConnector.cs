using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public float animSens = 1f;
    public float animWidth = 0.9f;
    public GameObject[] _objs;

    private LineRenderer line;
    private float tempT = 0.02f;
    private float tempW = 0.25f;
    private CollectableSc currentCollectable;
    AnimationCurve curve = new AnimationCurve();

    private void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
        curve = line.widthCurve;
    }

    private void Update()
    {
        //Debug.Log(line.widthCurve.keys[1].value);
        for(int i=0; i < _objs.Length; i++)
        {
            //Debug.Log(line.positionCount);
            line.SetPosition(i, _objs[i].transform.position + Vector3.up * 0.06f);
        }
    }

    public void PipeGetAnimTrigger(CollectableSc sc)
    {
        currentCollectable = sc;
        tempT = 0.05f;
        InvokeRepeating("PipeGetAnim", 0, Time.fixedDeltaTime);
    }

    private void PipeGetAnim()
    {
        if (tempT < 0.95f && line.widthCurve.keys[1].value < animWidth)
        {
            tempW = Mathf.MoveTowards(line.widthCurve.keys[1].value, animWidth, animSens * Time.deltaTime * 1.5f);
            MoveKeyFrame(1, 0.05f, tempW);
        }
        else if (tempT < 0.95f)
        {
            tempT += Time.deltaTime * animSens;
            MoveKeyFrame(1, tempT, animWidth);
        }
        else if (line.widthCurve.keys[1].value > 0.25f)
        {
            tempW = Mathf.MoveTowards(line.widthCurve.keys[1].value, 0.25f, animSens * Time.deltaTime * 1.5f);
            MoveKeyFrame(1, tempT, tempW);
        }
        else
        {
            MoveKeyFrame(1, 0.05f, 0.25f);
            currentCollectable.TakeTheFruit();
            CancelInvoke("PipeGetAnim");
        }
    }
    private void MoveKeyFrame(int index, float newTime, float newValue)
    {
        Keyframe[] keyFrames = curve.keys;

        keyFrames[index].time = newTime;
        keyFrames[index].value = newValue;

        curve.keys = keyFrames;
        line.widthCurve = curve;
    }

}
