using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPipeRenderSc : MonoBehaviour
{
    public LineConnector pipeLine;

    private void OnPreRender()
    {

        pipeLine.SetLinePositions();
    }
    private void OnPostRender()
    {
        pipeLine.SetLinePositions();
    }
}
