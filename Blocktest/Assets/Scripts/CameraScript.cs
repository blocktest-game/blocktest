// unset

using System;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private static Transform Target => Globals.characterObject.transform;

    private void LateUpdate()
    {
        if (Target is null) { return; }
        Vector3 targetPosition = Target.position;
        transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z - 10);
    }

}