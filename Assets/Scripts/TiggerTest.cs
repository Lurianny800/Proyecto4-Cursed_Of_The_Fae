using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Algo ha entrado en el trigger");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Jugador ha entrado en el trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Algo ha salido del trigger");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Jugador ha salido del trigger");
        }
    }
}
