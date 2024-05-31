using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a cargar

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el personaje tenga la etiqueta "Player"
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
