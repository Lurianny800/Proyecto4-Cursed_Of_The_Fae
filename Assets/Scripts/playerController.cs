using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float velocidad = 5f;
    private Animator animator;
    private Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        // Obtener la entrada del teclado
        float movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimientoVertical = Input.GetAxisRaw("Vertical");

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

        // Calcular el vector de movimiento
        Vector2 movimiento = new Vector2(movimientoHorizontal, movimientoVertical);
        // Mover el personaje
        MoverPersonaje(movimiento);
    }

    void MoverPersonaje(Vector2 direccion)
    {

        // Calcular la nueva posición sumando la dirección multiplicada por la velocidad y el tiempo
        Vector2 nuevaPosicion = rb.position + direccion * velocidad * Time.fixedDeltaTime;

        // Aplicar la nueva posición al personaje
        rb.MovePosition(nuevaPosicion);
    }   

   
}
