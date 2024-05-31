using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    // Referencias de UI
    [SerializeField]
    private GameObject dialogueCanvas;

    [SerializeField]
    private TMP_Text speakerText;

    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private Image portraitImage;

    // Contenido del diálogo
    [SerializeField]
    private string[] speaker;

    [SerializeField]
    [TextArea]
    private string[] dialogueWords;

    [SerializeField]
    private Sprite[] portrait;

    private bool dialogueActivated;
    private int step;

    void Start()
    {
        dialogueCanvas.SetActive(false);
        dialogueActivated = false;
        step = 0;
        Debug.Log("Diálogo inicializado");
    }

    void Update()
    {
        // Verificar si se presiona el botón "Interactuar"
        if (Input.GetKeyUp(KeyCode.M))
        {
            Debug.Log("Botón Interactuar presionado");
        }

        if (Input.GetButtonDown("Interactuar") && dialogueActivated)
        {
            Debug.Log("Interacción detectada y diálogo activado");
            if (step >= speaker.Length)
            {
                dialogueCanvas.SetActive(false);
                step = 0;
                dialogueActivated = false;
                Debug.Log("Diálogo terminado");
            }
            else
            {
                dialogueCanvas.SetActive(true);
                speakerText.text = speaker[step];
                dialogueText.text = dialogueWords[step];
                portraitImage.sprite = portrait[step];
                Debug.Log("Mostrando diálogo paso: " + step);
                step++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Jugador ha entrado en el trigger");
            dialogueActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Jugador ha salido del trigger");
            dialogueActivated = false;
            dialogueCanvas.SetActive(false);
        }
    }
}
