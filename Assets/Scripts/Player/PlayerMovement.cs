using System.Collections;
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

    ZMoverScript zObject;

    public int ZPlane = 0;

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
        if (GameController.ReturnConsoleState() == false)
        {
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /*
                //Realizar un check de si tenemos el suelo debajo
                if(Physics.Raycast(floorRay, out floorHit, 1.2f, 1 << groundLayer))
                {
                    print("Salto!");
                    //myBody.AddForce(0, jumpSpeed * 50f, 0);
                    myBody.velocity = new Vector3(0, jumpSpeed, 0);
                }
                ^*/
                Collider[] hitCollider1 = Physics.OverlapSphere(transform.position + new Vector3(.2f, -1f, 0), 0.1f, 1 << groundLayer);
                Collider[] hitCollider2 = Physics.OverlapSphere(transform.position + new Vector3(-.2f, -1f, 0), 0.1f, 1 << groundLayer);
                if (hitCollider1.Length > 0 || hitCollider2.Length > 0)
                {
                    print("Salto!");
                    //myBody.AddForce(0, jumpSpeed * 50f, 0);
                    myBody.velocity = new Vector3(0, jumpSpeed, 0);
                }
            }

            //Moverse en el eje Z (moverse al fondo)
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (zObject != null)
                {
                    if (zObject.moveUp)
                        ZMove(true);
                }
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (zObject != null)
                {
                    if (!zObject.moveUp)
                        ZMove(false);
                }
            }
        }


        myBody.velocity = new Vector3(xSpeed, myBody.velocity.y, myBody.velocity.z);
	}

    private void ZMove(bool up)
    {
        if (up)
            ZPlane++;
        else
            ZPlane--;

        transform.position = new Vector3(transform.position.x, transform.position.y, zObject.targetZMover.transform.position.z);
        print(ZPlane);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZMover"))
        {
            print("Inside ZMover");
            zObject = other.GetComponent<ZMoverScript>();
            zObject.ToggleArrowVisibility(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZMover"))
        {
            zObject.ToggleArrowVisibility(false);
            zObject = null;
        }
    }
}
