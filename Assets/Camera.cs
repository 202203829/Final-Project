using UnityEngine;

public class Camera : MonoBehaviour
{
    // Velocidad de movimiento
    public float moveSpeed = 1f;

    // Update se llama una vez por frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * moveSpeed;
    }
}
