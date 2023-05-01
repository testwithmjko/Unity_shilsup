using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialogueBox;
    public GameObject continueButton;
    public string[] dialogueLines;
    public int currentLine;

    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed = 0.02f; // 타이핑 속도를 조절하는 변수

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            if (!isTyping)
            {
                currentLine++;
                if (currentLine < dialogueLines.Length)
                {
                    StartCoroutine(TypeDialogue());
                }
                else
                {
                    dialogueBox.SetActive(false);
                }
            }
            else
            {
                cancelTyping = true;
            }
        }
    }

    public void StartDialogue()
    {
        dialogueBox.SetActive(true);
        currentLine = 0;
        StartCoroutine(TypeDialogue());
    }

    IEnumerator TypeDialogue()
    {
        isTyping = true;
        cancelTyping = false;
        dialogueText.text = "";
        foreach (char letter in dialogueLines[currentLine].ToCharArray())
        {
            dialogueText.text += letter;
            if (cancelTyping)
            {
                dialogueText.text = dialogueLines[currentLine];
                break;
            }
            yield return new WaitForSeconds(typeSpeed); // 타이핑 속도에 따라 기다립니다.
        }
        isTyping = false;
    }
}
