using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraScript : MonoBehaviour {


    public Transform target;
    private PlayerMovement player;

    private float xPos;
    private float yPos;
    private float zPos;

    private float xPosTo;
    private float yPosTo;
    private float zPosTo;

    private float xSpeed = 0.15f;
    private float ySpeed = 0.06f;
    private float zSpeed = 0.1f;

    //private float distToPlayer;

    private float DepthOffset = -7f;
    //private float VerticalOffset = 1f;
    private float HorizontalOffset = 0;

    private Vector3 cameraRot;
    //POST PROCESADO
    private DepthOfField dofLayer = null;
    private PostProcessVolume postVolume;
    private float distToPlayer;
    

    private Vector3 pos;
    private void Start()
    {
        player = target.GetComponent<PlayerMovement>();
        xPosTo = target.position.x + (HorizontalOffset * player.movRotation.x) + (DepthOffset * player.movRotation.z);
        yPosTo = target.position.y;
        zPosTo = target.position.z + (DepthOffset * player.movRotation.x);

        postVolume = gameObject.GetComponent<PostProcessVolume>();
        postVolume.profile.TryGetSettings(out dofLayer);
    }


    // Update is called once per frame
    void FixedUpdate () {
        distToPlayer = Vector3.Distance(transform.position, target.position);

        /*
        xPosTo = target.position.x;
        yPosTo = target.position.y + 3.2f + player.ZPlane;
        zPosTo = target.position.z - 8.5f - player.ZPlane*1.5f;
        */

        xPosTo = target.position.x + (HorizontalOffset * player.movRotation.x * GameController.LookDepth) + (-DepthOffset * player.movRotation.z * GameController.LookDepth);
        yPosTo = target.position.y + 1.5f;
        zPosTo = target.position.z + (DepthOffset * player.movRotation.x * GameController.LookDepth);

        xPos += (xPosTo - xPos) * xSpeed;
        yPos += (yPosTo - yPos) * ySpeed;
        zPos += (zPosTo - zPos) * zSpeed;
        
        pos = new Vector3(xPos, yPos, zPos);
        transform.position = pos;
        transform.LookAt(target);
        cameraRot = transform.rotation.eulerAngles;
        cameraRot = new Vector3(10f, cameraRot.y, cameraRot.z);
        transform.rotation = Quaternion.Euler(cameraRot);

        dofLayer.focusDistance.value = distToPlayer;
	}
}
