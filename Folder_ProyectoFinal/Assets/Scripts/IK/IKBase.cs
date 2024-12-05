using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AimIK.Properties;
using AimIK.Functions;
[ExecuteInEditMode]
public class IKBase : MonoBehaviour
{
    protected Animator anim;
    public Transform target;
    public bool IKActive = false;
    public Part[] IKPart;

    
    [ExecuteInEditMode]
    private void LateUpdate()
    {
        if (IKActive)
        {
            foreach (Part item in IKPart)
            {
                item.part.LookAt3D(target.position - item.positionOffset, item.rotationOffset);
                item.part.CheckClamp3D(item.limitRotation, item.GetRotation());
            }
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(target.position, 2f);
    //}



}
