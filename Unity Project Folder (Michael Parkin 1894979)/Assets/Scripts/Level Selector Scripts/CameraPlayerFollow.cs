using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{

    [Header("Inspector Config")]
    public Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3( playerTransform.transform.position.x, playerTransform.transform.position.y, transform.position.z);
    }
}
