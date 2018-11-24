using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody myBody;

    private float xSpeed = 0;
    private float xSpeedTo = 0;
    private float xSpeedFinal = 0;
    private float maxXSpeed = 10f;
    private float ySpeed = 0;

    public float jumpSpeed = 8f;
    public float movementSpeed = 8f;
    public float movementAccelerationOriginal = 1f;
    private float movementAccelarationNow = 1f;

    public bool isInAir;
    private Collider[] floorColliders;
    private Vector3 floorCollisionBoxSize;

    public int facingDirection = 1; //-1=izquierda ; 1=derecha

    private bool isFacingWall = false;

    Ray floorRay;
    RaycastHit floorHit;

    //VARIABLES FOR ZMOVING
    ZMoverScript zObject;
    public int ZPlane = 0;
    public float ZPlaneAnimationState = 0;
    private bool inZPlaneAnimation = false;
    private Vector3 ZPlaneStartPosition;
    private Vector3 ZPlaneEndPosition;
    private Vector3 ZPlaneMidPoint;
    private Vector3 tempPosition;

    //VARIABLES FOR HOLDING TO WALLS
    public bool isHoldingWall = false;
    private Vector3 holdingWallPos;
    private Vector3 wallCollisionBoxSize;
    private float holdingWallDelay = 0;

    //LAYERS
    int groundLayer = 8;
    int holdingWallLayer = 10;

	// Use this for initialization
	void Start () {
        myBody = GetComponent<Rigidbody>();
        wallCollisionBoxSize = new Vector3(0.3f, 0.35f, 0.2f);
        floorCollisionBoxSize = new Vector3(0.3f, 0.35f, 0.2f);
        holdingWallPos = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        floorColliders = Physics.OverlapBox(transform.position + new Vector3(0, -0.8f, 0), floorCollisionBoxSize, Quaternion.identity, (1 << groundLayer) | (1 << holdingWallLayer));
        if (floorColliders.Length > 0)
            isInAir = false;
        else
            isInAir = true;
        //Movimiento horizontal

        if (isInAir)
            movementAccelarationNow = movementAccelerationOriginal * 1f;
        else
            movementAccelarationNow = movementAccelerationOriginal;

        xSpeedFinal = xSpeedTo;
        if (xSpeedFinal > maxXSpeed)
            xSpeedFinal = maxXSpeed;
        else if (xSpeedFinal < -maxXSpeed)
            xSpeedFinal = -maxXSpeed;

        if(xSpeed < xSpeedFinal)
        {
            xSpeed += movementAccelarationNow;
            if (xSpeed > xSpeedFinal)
                xSpeed = xSpeedFinal;
        }
        else if(xSpeed > xSpeedFinal)
        {
            xSpeed -= movementAccelarationNow;
            if (xSpeed < xSpeedFinal)
                xSpeed = xSpeedFinal;
        }

        if (isHoldingWall)
        {
            myBody.velocity = Vector3.zero;
            myBody.useGravity = false;
        } else
        {
            if (holdingWallDelay > 0)
                holdingWallDelay -= Time.deltaTime;
            myBody.useGravity = true;
        }

        if(GameController.ReturnConsoleState() == true)
        {
            xSpeedTo = 0;
        }

        if (GameController.ReturnConsoleState() == false || inZPlaneAnimation)
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (!isHoldingWall)
                {
                    facingDirection = 1;
                    xSpeedTo += movementAccelarationNow * facingDirection;
                    Collider[] wallCollider = Physics.OverlapBox(transform.position + new Vector3(0.5f * facingDirection, 0.8f, 0), wallCollisionBoxSize, Quaternion.identity, 1 << holdingWallLayer);
                    if (wallCollider.Length > 0 && holdingWallDelay <= 0)
                        isHoldingWall = true;
                }
            }

            else if (Input.GetKey(KeyCode.A))
            {
                if (!isHoldingWall)
                {
                    facingDirection = -1;
                    xSpeedTo += movementAccelarationNow * facingDirection;
                    Collider[] wallCollider = Physics.OverlapBox(transform.position + new Vector3(0.5f * facingDirection, 0.8f, 0), wallCollisionBoxSize, Quaternion.identity, 1 << holdingWallLayer);
                    if (wallCollider.Length > 0 && holdingWallDelay <= 0)
                        isHoldingWall = true;
                }
            }

            //Parar movimiento horizontal al soltar alguna tecla de movimiento
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                xSpeedTo = 0;
            }

            

            //Salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isHoldingWall)
                {
                    isHoldingWall = false;
                    myBody.velocity = new Vector3(0, jumpSpeed, 0);
                    xSpeed = 7.8f * -facingDirection;
                    holdingWallDelay = 0.25f;
                }

                if (!isInAir && !isHoldingWall)
                {
                    myBody.velocity = new Vector3(0, jumpSpeed, 0);
                }
            }

            //Moverse en el eje Z (moverse al fondo)
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (zObject != null)
                {
                    if (zObject.moveUp && !zObject.isDisabled)
                        ZMove(true);
                }
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (zObject != null)
                {
                    if (!zObject.moveUp && !zObject.isDisabled)
                        ZMove(false);
                }
            }
        }

        if (inZPlaneAnimation)
        {
            
            tempPosition = Vector3.Lerp(ZPlaneStartPosition, ZPlaneEndPosition, ZPlaneAnimationState);
            if (ZPlaneMidPoint == Vector3.zero)
            {
                ZPlaneAnimationState += Time.deltaTime * 2.5f;
                tempPosition += new Vector3(0, 1.35f * Mathf.Sin(ZPlaneAnimationState * 3f), 0);
            }
            else
            {
                ZPlaneAnimationState += Time.deltaTime * 1.5f;
                tempPosition += new Vector3(0, 1f * Mathf.Abs(Mathf.Sin(ZPlaneAnimationState * 6f)), 0);
            }
            transform.position = tempPosition;

            if(ZPlaneAnimationState > 1)
            {
                ZPlaneAnimationState = 0;
                inZPlaneAnimation = false;
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

        inZPlaneAnimation = true;
        ZPlaneStartPosition = transform.position;
        
        if (zObject.midPoint != null)
        {
            ZPlaneMidPoint = zObject.midPoint.position;
            ZPlaneEndPosition = zObject.targetZMover.position;
        }
        else
        {
            ZPlaneMidPoint = Vector3.zero;
            ZPlaneEndPosition = zObject.targetZMover.position + new Vector3(0, 0.3f, 0);
        }
        ZPlaneAnimationState = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZMover"))
        {
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
