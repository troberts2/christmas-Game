using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    [SerializeField]private GameObject panel;

    // Start is called before the first frame update
    void OnEnable()
    {
        textComponent.text = string.Empty;
    }


    // Update is called once per frame
    void Update()
    {
            // if (textComponent.text == lines[index])
            // {
            //     NextLine();
            // }
            // else
            // {
            //     StopAllCoroutines();
            //     textComponent.text = lines[index];
            // }
    }

    public void StartDialogue()
    {
        index = 0;
        panel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(3f);
        textComponent.text = string.Empty;
        panel.SetActive(false);
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            panel.SetActive(false);
        }
    }
}