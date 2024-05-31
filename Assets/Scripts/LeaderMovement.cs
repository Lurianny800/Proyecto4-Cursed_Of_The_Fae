using UnityEngine;

public class LeaderMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the leader.");
        }
    }

    void Update()
    {
        // Movimiento de prueba para el l�der
        rb.velocity = new Vector2(1, 0); // Mueve al l�der a la derecha
    }
}
