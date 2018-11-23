﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody myBody;

    private float xSpeed = 0;
    private float ySpeed = 0;

    public float jumpSpeed = 8f;
    public float movementSpeed = 8f;

    Ray floorRay;
    RaycastHit floorHit;

    //LAYERS
    int groundLayer = 8;

	// Use this for initialization
	void Start () {
        myBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        floorRay = new Ray(transform.position, -Vector3.up);

        //Movimiento horizontal
        if (Input.GetKey(KeyCode.D))
        {
            xSpeed = movementSpeed;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            xSpeed = -movementSpeed;
        }

        //Parar movimiento horizontal al soltar alguna tecla de movimiento
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            xSpeed = 0;
        }

        //Salto
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Realizar un check de si tenemos el suelo debajo
            if(Physics.Raycast(floorRay, out floorHit, 1.2f, 1 << groundLayer))
            {
                print("Salto!");
                //myBody.AddForce(0, jumpSpeed * 50f, 0);
                myBody.velocity = new Vector3(0, jumpSpeed, 0);
            }
        }


        myBody.velocity = new Vector3(xSpeed, myBody.velocity.y, myBody.velocity.z);
	}
}
