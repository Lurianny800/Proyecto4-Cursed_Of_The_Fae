using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuriFollorLeader : MonoBehaviour
{
    public Transform leader; // Referencia al l�der
    public float followDistance = 2f; // Distancia que el seguidor mantendr� respecto al l�der
    public float followSpeed = 5f; // Velocidad de movimiento del seguidor
    private Rigidbody2D rb; // Referencia al Rigidbody2D del seguidor
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Variables de movimiento

        float movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimientoVertical = Input.GetAxisRaw("Vertical");

        // Verificar que el l�der est� asignado
        if (leader == null)
        {
            Debug.LogError("Leader is not assigned.");
            return;
        }

        // Calcular la distancia hacia el l�der
        Vector2 leaderPosition = leader.position;
        Vector2 currentPosition = rb.position;
        Vector2 direction = (leaderPosition - currentPosition).normalized;
        float distance = Vector2.Distance(leaderPosition, currentPosition);

        // Mover el seguidor hacia el l�der si la distancia es mayor que followDistance
        if (distance > followDistance)
        {
            Vector2 newPosition = Vector2.MoveTowards(currentPosition, leaderPosition - direction * followDistance, followSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // Animaciones en movimiento
            if (movimientoHorizontal != 0 || movimientoVertical != 0)
            {
                animator.SetFloat("X", movimientoHorizontal);
                animator.SetFloat("Y", movimientoVertical);
                animator.SetFloat("Speed", (float)0.1f);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }

            /*// Actualizar los par�metros del Animator basados en la direcci�n del movimiento
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", direction.sqrMagnitude);

            // Ajustar la animaci�n basada en la direcci�n del movimiento
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Movimiento horizontal
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", 0);
            }
            else
            {
                // Movimiento vertical
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", direction.y);
            }
        }
        else
        {
            // Si est� dentro de la distancia de seguimiento, detener la animaci�n de movimiento
            animator.SetFloat("Speed", 0);*/
        }
    }
}
