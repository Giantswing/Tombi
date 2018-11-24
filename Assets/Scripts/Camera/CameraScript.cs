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

    private float xSpeed = 0.06f;
    private float ySpeed = 0.03f;
    private float zSpeed = 0.1f;

    private float distToPlayer;


    //POST PROCESADO
    //private DepthOfField dofLayer = null;
    //private PostProcessVolume postVolume;
    

    private Vector3 pos;
    private void Start()
    {
        xPosTo = target.position.x;
        yPosTo = target.position.y + 1f;
        zPosTo = target.position.z - 7f;

        player = target.GetComponent<PlayerMovement>();

        //postVolume = gameObject.GetComponent<PostProcessVolume>();
        //postVolume.profile.TryGetSettings(out dofLayer);
    }


    // Update is called once per frame
    void FixedUpdate () {
        distToPlayer = Vector3.Distance(transform.position, target.position);

        xPosTo = target.position.x;
        yPosTo = target.position.y + 3.2f + player.ZPlane;
        zPosTo = target.position.z - 8.5f - player.ZPlane*1.5f;

        xPos += (xPosTo - xPos) * xSpeed;
        yPos += (yPosTo - yPos) * ySpeed;
        zPos += (zPosTo - zPos) * zSpeed;
        
        pos = new Vector3(xPos, yPos, zPos);
        transform.position = pos;

        //dofLayer.focusDistance.value = distToPlayer;
	}
}
