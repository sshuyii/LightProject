using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<Transform> targets;
  
    private void LateUpdate() 
    {
        Vector3 centerPoint = GetCenterPoint();
    }

    private Vector3 GetCenterPoint()
    {
        Vector3 temp = Vector3.zero;

        return temp;
    }
}
