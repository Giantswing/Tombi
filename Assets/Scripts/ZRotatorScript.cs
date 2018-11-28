using UnityEngine;

public class ZRotatorScript : MonoBehaviour
{
    public GameObject myArrows;
    private bool imActive = false;
    private bool showingArrow = false;
    private float arrowScale;
    private float arrowScaleTo;
    public bool isDisabled = false;
    //public Vector3 newRot;
    public bool moveUp;

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
