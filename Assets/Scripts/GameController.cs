using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject player;

    private Vector3 startingPos;

    private void Start()
    {
        startingPos = player.transform.position;
    }
    void Update () {
        if (player.transform.position.y < -10f)
        {
            RestartGame();
        }
	}

    void RestartGame()
    {
        player.transform.position = startingPos;
    }
}
