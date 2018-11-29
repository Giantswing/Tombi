using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController me;

    public string gameVersion;

    public GameObject player;
    public Text debugText;
    private Vector3 startingPos;
    private PlayerMovement playerScript;
    private Rigidbody playerBody;

    private float deltaTime;
    private float fpsTemp;
    private float fpsFinal;
    private float memoryAllocated;

    //VARIABLES DE LA CONSOLA
    public bool ConsoleMode = false;
    public GameObject[] ConsoleObjects;
    public string[] ConsoleMessages;
    public Text ConsoleLog;
    public InputField ConsoleInputField;

    private void Start()
    {
        me = GetComponent<GameController>();

        startingPos = player.transform.position;
        playerScript = player.GetComponent<PlayerMovement>();
        playerBody = player.GetComponent<Rigidbody>();


        ConsoleMessages = new string[10];
        for (int i = 0; i < ConsoleMessages.Length; i++)
        {
            ConsoleMessages[i] = "";
        }

        ConsoleMessages[0] = "Console, Tombi version " + gameVersion + " | Type all to see all commands";

        if (ConsoleMode)
            ToggleConsole();

    }
    void Update () {

        if (player.transform.position.y < -10f)
        {
            RestartGame();
        }


        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fpsTemp = 1.0f / deltaTime;
        fpsFinal = Mathf.Ceil(fpsTemp);

        debugText.text = "Tombi " + gameVersion + "  Press TAB to toggle the console\nFPS: " + fpsFinal +"\nPlayer position: " + player.transform.position +
            "\nPlayer velocity: " + playerBody.velocity + "\nPlayer movRotation/rotation: " + playerScript.movRotation + "|" + playerScript.transform.rotation.eulerAngles + "\nPlayer ZPlane: " + playerScript.ZPlane
            +"\nHolding wall: " + playerScript.isHoldingWall;

        if (Input.GetButtonDown("ConsoleToggle"))
        {
            GameController.ToggleConsole();
        }


        if (ConsoleMode)
        {
            ConsoleInputField.Select();
            ConsoleInputField.ActivateInputField();
            if (Input.GetButtonDown("ConsoleEnter"))
            {
                bool messageWritten = false;

                if (ConsoleInputField.text != "")
                {
                    for (int i = 0; i < ConsoleMessages.Length; i++)
                    {
                        if (ConsoleMessages[i] == "")
                        {
                            ConsoleMessages[i] = ConsoleInputField.text;
                            messageWritten = true;
                            break;
                        }
                    }
                    if (!messageWritten)
                    {
                        for (int i = 0; i < ConsoleMessages.Length; i++)
                        {
                            if (i == ConsoleMessages.Length - 1)
                            {
                                ConsoleMessages[i] = ConsoleInputField.text;
                                messageWritten = true;
                            }
                            else
                                ConsoleMessages[i] = ConsoleMessages[i + 1];
                        }
                    }
                    if (messageWritten)
                    {
                        GameController.ExecuteConsoleCommand(ConsoleInputField.text);
                    }
                    ConsoleInputField.text = "";
                    ConsoleInputField.Select();
                    ConsoleInputField.ActivateInputField();
                    //MasterScript.ToggleConsole();

                }
            }
        }

        ConsoleLog.text = "";
        for (int i = 0; i < ConsoleMessages.Length; i++)
        {
            ConsoleLog.text += ConsoleMessages[i] + "\n";
        }
    }

    void RestartGame()
    {
        print("YOU DIED!");
        player.transform.position = startingPos;
        playerScript.ZPlane = 0;
        playerScript.isHoldingWall = false;
        playerScript.movRotation = new Vector3(1f, 0, 0);
        player.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public static bool ReturnConsoleState()
    {
        return me.ConsoleMode;
    }

    public static void WriteLineInConsole(string line)
    {
        bool messageWritten = false;

        if(me.ConsoleInputField.text != "")
        {
            for (int i=0; i<me.ConsoleMessages.Length; i++)
            {
                if (me.ConsoleMessages[i] == "")
                {
                    me.ConsoleMessages[i] = line;
                    messageWritten = true;
                    break;
                }
            }

            if (!messageWritten)
            {
                for (int i=0; i<me.ConsoleMessages.Length; i++)
                {
                    if (i == me.ConsoleMessages.Length - 1)
                    {
                        me.ConsoleMessages[i] = line;
                        messageWritten = true;
                    }
                    else
                        me.ConsoleMessages[i] = me.ConsoleMessages[i + 1];
                }
            }

            me.ConsoleInputField.Select();
            me.ConsoleInputField.ActivateInputField();
        }
    }

    public static void ExecuteConsoleCommand(string command)
    {
        command = command.ToLower();
        string[] fcommand = command.Split(new string[] { " " }, System.StringSplitOptions.None);

        switch (fcommand[0])
        {
            case "exit":
                GameController.WriteLineInConsole("Saliendo del juego...");
                Application.Quit();
                break;

            case "restartplayer":
                me.RestartGame();
                break;

            case "all":
                GameController.WriteLineInConsole("exit | restartplayer | all | maxfps fps");
                break;

            case "maxfps":
                int tempCheck = int.Parse(fcommand[1]);
                if (tempCheck > 5)
                {
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = tempCheck;
                    GameController.WriteLineInConsole("Fps máximos cambiados a " + tempCheck);
                }
                break;
        }
    }

    public static void ToggleConsole()
    {
        if (me.ConsoleMode == false)
        {
            for (int i = 0; i < me.ConsoleObjects.Length; i++)
            {
                me.ConsoleObjects[i].SetActive(true);
            }
            me.ConsoleMode = true;
            me.ConsoleInputField.Select();
            me.ConsoleInputField.ActivateInputField();
        }

        else if (me.ConsoleMode == true)
        {
            for (int i = 0; i < me.ConsoleObjects.Length; i++)
            {
                me.ConsoleObjects[i].SetActive(false);
            }
            me.ConsoleMode = false;
        }
    }



}
