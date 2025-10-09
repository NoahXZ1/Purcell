using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;     // target = Player
    public float smooth = 8f;
    public Vector2 offset = new Vector2(2f, 1.5f);

    void LateUpdate()
    {
        if (!target) return;
        Vector3 desired = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desired, 1 - Mathf.Exp(-smooth * Time.deltaTime));
    }
}
