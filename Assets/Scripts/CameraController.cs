using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _target;
    public float smoothSpeed = 0.125f; 
    public Vector3 locationOffset;
    public Vector3 rotationOffset;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = _target.position + _target.rotation * locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Quaternion desiredRotation = _target.rotation * Quaternion.Euler(rotationOffset);
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed);
        transform.rotation = smoothedRotation;
    }

    public void SetTarget(PlayerController player)
    {
        _target = player.transform;
    }
}
