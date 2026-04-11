using UnityEngine;
//This is a little Easter egg I made lol, I like the rotating portal.
public class Rotator : MonoBehaviour
{
    [Tooltip("Degrees per second. Positive = counter-clockwise, negative = clockwise.")]
    public float speed = -60f;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
