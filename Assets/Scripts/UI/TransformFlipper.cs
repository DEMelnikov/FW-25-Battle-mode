using UnityEngine;

public class TransformFlipper : MonoBehaviour
{
    private Vector3 lastPosition;
    void Start()
    {
        lastPosition = transform.position;
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (PauseManager.IsPaused) return;

        Vector3 movement = transform.position - lastPosition;

        if (movement.x > 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (movement.x < -0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        lastPosition = transform.position;
    }
}

