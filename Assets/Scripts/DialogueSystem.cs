using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour {
    public PlayerMovement player;
    public GameObject textPopup;
    public float textSpeed;

    [TextArea(5, 10)]
    public string[] Lines;

    private int actualLine = 0;
    public bool[] Requeriments;

    private void Start()
    {
        //print(gameObject.name);
        TogglePopup(false);
    }

    public void TogglePopup(bool state)
    {
        textPopup.SetActive(state);
    }

    public void StartDialogue()
    {
        DialogueMaster.ToggleDialogue(true);
        DialogueMaster.me.textSpeed = textSpeed;
        TogglePopup(false);
    }

    public void Talk()
    {
        if (actualLine != Lines.Length)
        {
            Say(Lines[actualLine]);
            actualLine++;
        }
        else
        {
            TogglePopup(true);
            DialogueMaster.ToggleDialogue(false);
            player.isTalking = false;
            actualLine = 0;
            DialogueMaster.SetLine("");
        }
    }

    private void Say(string line)
    {
        DialogueMaster.SetLine(line);
    }
}
