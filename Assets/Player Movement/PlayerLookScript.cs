using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
    Vector2 rotation = Vector2.zero;
    public float speed = 3;

    void Update()
    {
        rotation.y += Input.GetAxis("Mouse X");
        transform.eulerAngles = (Vector2)rotation * speed;
    }
}
