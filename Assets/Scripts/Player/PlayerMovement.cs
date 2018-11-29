using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody myBody;

    private float xSpeed = 0;
    private float xSpeedTo = 0;
    private float xSpeedFinal = 0;
    
    private float ySpeed = 0;

    public float jumpSpeed = 8f;
    public float maxXSpeed = 10f;
    public float movementAccelerationOriginal = 1f;
    private float movementAccelarationNow = 1f;
    private float movementBrakeOriginal = 1.5f;
    private float movementBrakeNow = 1.5f;
    private bool isMoving = false;

    [HideInInspector]
    public bool isInAir;
    private Collider[] floorColliders;
    private Collider[] holdingWallColliders;
    private Collider[] normalWallColliders;
    private Vector3 floorCollisionBoxSize;

    public int facingDirection = 1; //-1=izquierda ; 1=derecha

    private bool isFacingWall = false;

    Ray floorRay;
    RaycastHit floorHit;

    //VARIABLES FOR ZMOVING
    ZMoverScript zObject;
    ZRotatorScript zRotator;
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

    private Vector3 holdingWallCollisionBoxSize;
    private Vector3 normalWallCollisionBoxSize;

    private float holdingWallDelay = 0;

    //LAYERS
    int groundLayer = 8;
    int holdingWallLayer = 10;

    //VARIABLES FOR ZROTATING
    public Vector3 movRotation;
    private Vector3 playerRotation;

	// Use this for initialization
	void Start () {
        playerRotation = Vector3.zero;
        movRotation = new Vector3(1f, 0, 0);
        myBody = GetComponent<Rigidbody>();

        holdingWallCollisionBoxSize = new Vector3(0.3f, 0.35f, 0.2f);
        normalWallCollisionBoxSize = new Vector3(0.2f, 0.1f, 0.1f);
        floorCollisionBoxSize = new Vector3(0.35f, 0.35f, 0.2f);
        holdingWallPos = Vector3.zero;
    }

    private void FixedUpdate()
    {

        floorColliders = Physics.OverlapBox(transform.position + new Vector3(0, -0.8f, 0), floorCollisionBoxSize, Quaternion.identity, (1 << groundLayer) | (1 << holdingWallLayer));
        if (floorColliders.Length > 0)
            isInAir = false;
        else
            isInAir = true;


        holdingWallColliders = Physics.OverlapBox(transform.position + new Vector3((0.5f * facingDirection * movRotation.x), 0.8f, (0.5f * facingDirection * movRotation.z)), holdingWallCollisionBoxSize, Quaternion.identity, 1 << holdingWallLayer);
        //holdingWallColliders = Physics.OverlapBox(transform.position + new Vector3((0.5f * facingDirection), 0.8f, 0), holdingWallCollisionBoxSize, Quaternion.identity, 1 << holdingWallLayer);
        if (holdingWallColliders.Length == 0)
                isHoldingWall = false;
        
        
        normalWallColliders = Physics.OverlapBox(transform.position + new Vector3((0.5f * facingDirection * movRotation.x), 0.5f, (0.5f * facingDirection * movRotation.z)), normalWallCollisionBoxSize, Quaternion.identity, 1 << groundLayer);
            if (normalWallColliders.Length > 0 || isHoldingWall)
                xSpeed = 0;     
        
        
    }


    // Update is called once per frame
    void Update () {

        //Movimiento horizontal
        if (isInAir)
        {
            movementAccelarationNow = movementAccelerationOriginal * 1f;
            movementBrakeNow = movementBrakeOriginal * 0.1f;
        }
        else
        {
            movementAccelarationNow = movementAccelerationOriginal;
            movementBrakeNow = movementBrakeOriginal;
        }

        if (!isMoving)
        {
            if(xSpeed > 0)
            {
                xSpeed -= movementBrakeNow * Time.deltaTime * 60f;
                if (xSpeed < 0)
                    xSpeed = 0;
            }
            else if(xSpeed < 0)
            {
                xSpeed += movementBrakeNow * Time.deltaTime * 60f;
                if (xSpeed > 0)
                    xSpeed = 0;
            }
        }

        if (xSpeed > maxXSpeed)
            xSpeed = maxXSpeed;
        else if (xSpeed < -maxXSpeed)
            xSpeed = -maxXSpeed;


        if (isHoldingWall)
        {
            myBody.velocity = Vector3.zero;
            myBody.useGravity = false;
            xSpeedTo = 0;
        } else
        {
            if (holdingWallDelay > 0)
                holdingWallDelay -= Time.deltaTime;
            myBody.useGravity = true;
        }

        if(GameController.ReturnConsoleState() == true)
        {
            isMoving = false;
            xSpeed = 0;
            xSpeedTo = 0;
        }

        if (GameController.ReturnConsoleState() == false || inZPlaneAnimation)
        {
            if (Input.GetKey(KeyCode.D))
            {
                if (!isHoldingWall)
                {
                    isMoving = true;
                    TurnDirection(1);
                    xSpeed += movementAccelarationNow * facingDirection * Time.deltaTime * 60f;

                    if (holdingWallColliders.Length > 0 && holdingWallDelay <= 0)
                        isHoldingWall = true;
                }
            }

            else if (Input.GetKey(KeyCode.A))
            {
                if (!isHoldingWall)
                {
                    isMoving = true;
                    TurnDirection(-1);
                    xSpeed += movementAccelarationNow * facingDirection * Time.deltaTime * 60f;
                    if (holdingWallColliders.Length > 0 && holdingWallDelay <= 0)
                        isHoldingWall = true;
                }
            }

            //Parar movimiento horizontal al soltar alguna tecla de movimiento
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                isMoving = false;
            }


            //Salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isHoldingWall)
                {
                    isHoldingWall = false;
                    myBody.velocity = new Vector3(0, jumpSpeed, 0);
                    xSpeed = (movementAccelarationNow*12f) * -facingDirection;
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

                else if(zRotator != null)
                {
                    if (!zRotator.isDisabled)
                    {
                        ZRotate();
                    }
                }
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (zObject != null)
                {
                    if (!zObject.moveUp && !zObject.isDisabled)
                        ZMove(false);
                }

                else if (zRotator != null)
                {
                    if (!zRotator.isDisabled)
                    {
                        ZRotate();
                    }
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

        //////////ACTUALIZAR VELOCIDAD DEL RIGIDBODY/////////////////////////////////////////////////////////////

        //myBody.velocity = new Vector3(xSpeed, myBody.velocity.y, myBody.velocity.z);
        myBody.velocity = new Vector3(xSpeed * movRotation.x, myBody.velocity.y, xSpeed * movRotation.z);
    }

    private void TurnDirection(int dir)
    {
        if (!isHoldingWall)
        {
            if (facingDirection != dir)
            {
                facingDirection = dir;
            }
            transform.localScale = new Vector3(dir, 1f, 1f);
        }
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

    private void ZRotate()
    {
        //movRotation = zRotator.newRot;

        /*
        if (zRotator.moveUp)
        {
            if(movRotation == new Vector3(1f, 0, 0))
            {
                movRotation = new Vector3()
            }
        }
        */

        if (movRotation == new Vector3(1f, 0, 0))
        {
            movRotation = new Vector3(0, 0, 1f);
            playerRotation = new Vector3(0, -90f, 0);
            transform.rotation = Quaternion.Euler(playerRotation);
        }
        else if (movRotation == new Vector3(0, 0, 1f))
        {
            movRotation = new Vector3(1f, 0, 0);
            playerRotation = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Euler(playerRotation);
        }

        Vector3 newPosition = new Vector3(zRotator.transform.position.x, 0, zRotator.transform.position.z);

        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZMover"))
        {
            zObject = other.GetComponent<ZMoverScript>();
            zObject.ToggleArrowVisibility(true);
        }

        else if (other.CompareTag("ZRotator"))
        {
            zRotator = other.GetComponent<ZRotatorScript>();
            zRotator.ToggleArrowVisibility(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZMover"))
        {
            zObject.ToggleArrowVisibility(false);
            zObject = null;
        }

        else if (other.CompareTag("ZRotator"))
        {
            zRotator.ToggleArrowVisibility(false);
            zRotator = null;
        }
    }
}
