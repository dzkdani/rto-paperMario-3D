using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float _typeSpdDelay;
    public Coroutine Run(string text, TMP_Text label)
    {
        return StartCoroutine(TypeText(text, label));
    }

    public IEnumerator TypeText(string text, TMP_Text label)
    {
        label.text = string.Empty;
        foreach (char letter in text.ToCharArray())
        {
            label.text += letter;
            yield return new WaitForSeconds(_typeSpdDelay);
        }


        //label.text = string.Empty;

        //float time = 0;
        //int charIdx = 0;

        //while (charIdx < text.Length)
        //{
        //    time += Time.deltaTime * _typeSpd;
        //    charIdx = Mathf.FloorToInt(time);
        //    charIdx  = Mathf.Clamp(charIdx, 0, text.Length);

        //    label.text = text.Substring(0, charIdx);

        //    yield return null;
        //}

        //label.text = text;
    }
}
