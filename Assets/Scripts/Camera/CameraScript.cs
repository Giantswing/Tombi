using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform target;

    /*
    public Vector3 offset;
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    public float smoothSpeed = 0.125f;
    */

    private float xPos;
    private float yPos;
    private float zPos;

    private float xPosTo;
    private float yPosTo;
    private float zPosTo;

    private float xSpeed = 0.06f;
    private float ySpeed = 0.03f;
    private float zSpeed = 0.06f;

    private Vector3 pos;
    private void Start()
    {
        xPosTo = target.position.x;
        yPosTo = target.position.y + 1f;
        zPosTo = target.position.z - 7f;
    }


    // Update is called once per frame
    void FixedUpdate () {
        xPosTo = target.position.x;
        yPosTo = target.position.y + 1.5f;
        zPosTo = target.position.z - 7f;

        xPos += (xPosTo - xPos) * xSpeed;
        yPos += (yPosTo - yPos) * ySpeed;
        zPos += (zPosTo - zPos) * zSpeed;
        
        pos = new Vector3(xPos, yPos, zPos);
        transform.position = pos;
	}
}
