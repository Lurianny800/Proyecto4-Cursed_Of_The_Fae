using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogController : MonoBehaviour
{
    public GameObject dialogPanel; // El panel que contiene el texto del diálogo.
    public TextMeshProUGUI textComponent; // El componente TextMeshProUGUI para mostrar el texto del diálogo.
    public Image characterImage; // El Image del sprite.
    public GameObject characterNamePanel; // El panel que contiene el texto del nombre del personaje.
    public TextMeshProUGUI characterNameText; // El componente TextMeshProUGUI para mostrar el nombre del personaje.
    public List<DialogueEntry> dialogueEntries; // Lista de entradas de diálogo.
    public float textSpeed = 0.05f; // La velocidad a la que se escribe el texto.
    private int index; // El índice de la línea actual.
    private bool isTyping = false; // Para verificar si se está escribiendo una línea.

    [System.Serializable]
    public class DialogueEntry
    {
        public string characterName;
        public string dialogueLine;
        public Sprite characterSprite;
    }
    void Start()
    {
        dialogPanel.SetActive(false);
        characterImage.gameObject.SetActive(false);
        characterNamePanel.SetActive(false);
        textComponent.text = string.Empty;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            if (!dialogPanel.activeInHierarchy)
            {
                dialogPanel.SetActive(true);
                characterImage.gameObject.SetActive(true);
                characterNamePanel.SetActive(true);
                StartDialogue();
            }
            else if (textComponent.text == dialogueEntries[index].dialogueLine)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = dialogueEntries[index].dialogueLine;
                isTyping = false;
            }
        }
    }
    void StartDialogue()
    {
        index = 0;
        SetCharacterData(index);
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;
        foreach (char c in dialogueEntries[index].dialogueLine.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }
    void NextLine()
    {
        if (index < dialogueEntries.Count - 1)
        {
            index++;
            SetCharacterData(index);
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogPanel.SetActive(false);
            characterImage.gameObject.SetActive(false);
            characterNamePanel.SetActive(false);
        }
    }
    void SetCharacterData(int currentIndex)
    {
        characterNameText.text = dialogueEntries[currentIndex].characterName;
        characterImage.sprite = dialogueEntries[currentIndex].characterSprite;
    }
}