using System.Collections;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;
    public string[] dialogs;
    private int currentDialogIndex = 0;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        ShowDialog();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Skip typing effect
                StopCoroutine(typingCoroutine);
                dialogText.text = dialogs[currentDialogIndex];
                isTyping = false;
            }
            else
            {
                // Display next dialog or end the conversation
                currentDialogIndex++;
                if (currentDialogIndex < dialogs.Length)
                {
                    ShowDialog();
                }
                else
                {
                    // End of conversation
                    Debug.Log("End of conversation");
                }
            }
        }
    }

    void ShowDialog()
    {
        dialogText.text = string.Empty;
        typingCoroutine = StartCoroutine(TypeText(dialogs[currentDialogIndex]));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        foreach (char letter in text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
}