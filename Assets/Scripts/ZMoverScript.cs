using UnityEngine;

public class ZMoverScript : MonoBehaviour {
    public GameObject myArrows;
    public Transform targetZMover;
    public Transform midPoint;
    public bool moveUp = true;
    private bool imActive = false;
    private bool showingArrow = false;
    private float arrowScale;
    private float arrowScaleTo;
    public bool isDisabled = false;

    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0.25f, 1f);
        //Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector3(1f, 1f, 1f));
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
    }
    

    public void ToggleArrowVisibility(bool show)
    {
        imActive = show;
    }

    private void Update()
    {
        if (!isDisabled)
        {
            if (imActive)
                arrowScaleTo = 1f;
            else
                arrowScaleTo = 0;

            if (showingArrow)
            {
                if (arrowScale < 0.1f)
                {
                    myArrows.SetActive(false);
                    showingArrow = false;
                }
            }
            else
            {
                if (arrowScale > 0.1f)
                {
                    myArrows.SetActive(true);
                    showingArrow = true;
                }
            }

            arrowScale += (arrowScaleTo - arrowScale) * .4f * Time.deltaTime * 60f;
            myArrows.transform.localScale = new Vector3(arrowScale, arrowScale, arrowScale);
        }

    }
}
