using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMaintainer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        Quaternion rotation = Quaternion.LookRotation(camera.transform.forward, camera.transform.up);
        transform.rotation = rotation;
    }
}
