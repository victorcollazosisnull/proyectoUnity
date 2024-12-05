using AimIK.Functions;
using AimIK.Properties;
using UnityEngine;
[ExecuteInEditMode]
public class IKHuman : IKBase
{
    public Transform LeftHand;
    public Transform RightHand;
    public Transform LeftElbow; // Nuevo: Transform para el codo izquierdo
    public Transform RightElbow; // Nuevo: Transform para el codo derecho
    public float radiusLeftHand;
    public float radiusRightHand;
    public float radiusTarget;
    public bool LaunchGranade;
    public bool Reloading;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    [ExecuteInEditMode]
    private void LateUpdate()
    {
        anim.Update(0);
        if (IKActive)
        {
            foreach (Part item in IKPart)
            {
                Vector3 IKPosition = (target.position - item.positionOffset);
                item.part.LookAt3D(IKPosition, item.rotationOffset);
                item.part.CheckClamp3D(item.limitRotation, item.GetRotation());
            }
        }
    }

    [ExecuteInEditMode]
    private void OnAnimatorIK(int layerIndex)
    {
        if (IKActive)
        {
            // Configuración de IK para la mano izquierda
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand.rotation);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand.position);

            // Control del codo izquierdo
            anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1f);
            anim.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftElbow.position);


            // Configuración de IK para la mano derecha
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKRotation(AvatarIKGoal.RightHand, RightHand.rotation);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.RightHand, RightHand.position);

            // Control del codo derecho
            anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1f);
            anim.SetIKHintPosition(AvatarIKHint.RightElbow, RightElbow.position);

            
        }
    }

    private void OnDrawGizmos()
    {
        if (LeftHand)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(LeftHand.position, radiusLeftHand);
        }
        if (RightHand)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(RightHand.position, radiusRightHand);
        }
        if (target)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(target.position, radiusTarget);
        }
    }
}
