using UnityEngine;

public class SkyRotation : MonoBehaviour
{
    public float speedRotation;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedRotation); 
    }
}
