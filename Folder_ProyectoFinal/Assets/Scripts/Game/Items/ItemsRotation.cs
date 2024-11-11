using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; 

    void Update()
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, rotationSpeed * Time.deltaTime);
        transform.rotation = transform.rotation * rotation;
    }
}