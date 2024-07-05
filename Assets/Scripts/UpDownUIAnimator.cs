using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpDownUIAnimator : MonoBehaviour
{
    public Vector2 limits = new Vector2(-700, -715);
    public float sens = 1;
    public float offset = 2;

    private bool dir = false;

    private void FixedUpdate()
    {
        Vector3 target = dir ? Vector3.up * (limits.x + offset) : Vector3.up * (limits.y - offset);
        target.x = transform.localPosition.x;
        target.z = transform.localPosition.z;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, sens * Time.deltaTime);

        if (dir && transform.localPosition.y > limits.x)
        {
            dir = false;
        }
        else if (!dir && transform.localPosition.y < limits.y)
        {
            dir = true;
        }
    }
}