using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueMaster : MonoBehaviour {
    public static DialogueMaster me;
    public TextMeshProUGUI dialogueText;
    public GameObject main;

    public float textSpeed;
    public float textPos = 0;
    public int currentLetter = 0;
    public GameObject nextIcon;

    public string CurrentText="";
    public string CurrentLine = "";
    private int CurrentLength = 0;
    private int skip = 0;
    

    private void Start()
    {
        nextIcon.SetActive(false);
        me = GetComponent<DialogueMaster>();
        DialogueMaster.ToggleDialogue(false);
        me.dialogueText.text = "";
    }

    public static void ToggleDialogue(bool state)
    {
        me.main.SetActive(state);
    }

    public static void SetLine(string line)
    {
        me.textPos = 1;
        me.CurrentLength = 0;
        me.currentLetter = 0;
        me.CurrentText = "";
        me.CurrentLine = line;
        //me.dialogueText.SetText(line);
    }

    private void Update()
    {
        if(CurrentLine != "")
        {
            //print("esta ocurriendo!");
            //print(CurrentLength);
            textPos += Time.deltaTime * textSpeed;
            if(textPos >= 1)
            {
                if (CurrentLength < CurrentLine.Length)
                {
                    if (CurrentLine[currentLetter] == '<')
                    {
                        for(skip=0; skip < 40; skip++)
                        {

                            if (CurrentLine[currentLetter + skip -1] == '>' && CurrentLine[currentLetter+skip] == ' ')
                            {
                                break;
                            }
                            else
                            {
                                CurrentText += CurrentLine[currentLetter + skip];
                                dialogueText.SetText(CurrentText);
                            }
                        }

                        currentLetter += skip+1;
                        CurrentLength += skip + 1;

                    }
                    if(CurrentLength+1 >= CurrentLine.Length)
                    {
                        nextIcon.SetActive(true);
                    }
                    else
                    {
                        nextIcon.SetActive(false);
                    }
                                    

                    textPos = 0;
                    CurrentText += CurrentLine[currentLetter];
                    dialogueText.SetText(CurrentText);
                    currentLetter++;
                    CurrentLength++;
                }
            }
        }
    }

}
