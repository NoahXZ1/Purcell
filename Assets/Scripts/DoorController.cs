using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController2D : MonoBehaviour
{
    public enum OpenDirection { Up, Down }

    [Header("Motion")]
    public OpenDirection openDirection = OpenDirection.Up;
    //The distance the door will move when press K
    public float openDistance = 2.5f;
    //Time of the whole animation(control the speed of door)
    public float openDuration = 0.4f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Colliders")]
    public Collider2D solidCollider;

    //Other script can read the value of IsOpen but cannot set it
    public bool IsOpen { get; private set; }
    bool _opening;

    void Awake()
    {
        if (!solidCollider) solidCollider = GetComponent<Collider2D>();
    }

    public void TryOpen()
    {
        //Disable the open door function when opening or opened. 
        if (IsOpen || _opening)
        {
            return;
        }
        StartCoroutine(OpenRoutine());
    }

    IEnumerator OpenRoutine()
    {
        _opening = true;

        //Record the start position of the door
        Vector3 start = transform.position;
        //Determine whether the door will move up or down
        Vector3 dir = (openDirection == OpenDirection.Up) ? Vector3.up : Vector3.down;
        //Compute the distance the door will move through
        Vector3 target = start + dir * openDistance;

        float t = 0f;
        if (solidCollider)
        {
            solidCollider.enabled = false;
        }

        //Make the animation run once every frame
        while (t < 1f)
        {
            //deltaTime: the time from last frame to current frame
            t += Time.deltaTime / Mathf.Max(0.01f, openDuration);
            float k = ease.Evaluate(Mathf.Clamp01(t));
            transform.position = Vector3.LerpUnclamped(start, target, k);
            yield return null;
        }

        transform.position = target;
        IsOpen = true;
        _opening = false;
    }
}

