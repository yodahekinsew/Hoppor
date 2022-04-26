using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynchronizeText : MonoBehaviour
{
    public List<TextMeshProUGUI> texts;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SynchronizeFontSize());
    }
    IEnumerator SynchronizeFontSize()
    {
        yield return new WaitForSeconds(.25f);

        float minFontSize = texts[0].fontSize;
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.fontSize < minFontSize) minFontSize = text.fontSize;
        }

        foreach (TextMeshProUGUI text in texts)
        {
            text.enableAutoSizing = false;
            text.fontSize = minFontSize;
        }
    }
}
