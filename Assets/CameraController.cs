using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 0.125f;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;
    }


}
