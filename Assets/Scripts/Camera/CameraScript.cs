using UnityEngine;
using UnityEngine.PostProcessing;
public class CameraScript : MonoBehaviour {

    private PostProcessingProfile myProfile;
    private DepthOfFieldModel.Settings dofModel;
    public Transform target;
    private PlayerMovement player;

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

    private float distToPlayer;

    private Vector3 pos;
    private void Start()
    {
        xPosTo = target.position.x;
        yPosTo = target.position.y + 1f;
        zPosTo = target.position.z - 7f;

        myProfile = GetComponent<PostProcessingBehaviour>().profile;
        dofModel = myProfile.depthOfField.settings;

        player = target.GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void FixedUpdate () {
        distToPlayer = Vector3.Distance(transform.position, target.position);

        xPosTo = target.position.x;
        yPosTo = target.position.y + 3.2f;
        zPosTo = target.position.z - 8.5f - player.ZPlane*1.5f;

        xPos += (xPosTo - xPos) * xSpeed;
        yPos += (yPosTo - yPos) * ySpeed;
        zPos += (zPosTo - zPos) * zSpeed;
        
        pos = new Vector3(xPos, yPos, zPos);
        transform.position = pos;

        dofModel.focusDistance = distToPlayer;

        myProfile.depthOfField.settings = dofModel;
	}
}
