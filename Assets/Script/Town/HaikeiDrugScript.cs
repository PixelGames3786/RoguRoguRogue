using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaikeiDrugScript : MonoBehaviour
{
    private RectTransform ThisRect;

    private Vector3 MotoMousePosition,NowMousePosition;

    private bool OnMouseDown;

    // Start is called before the first frame update
    void Start()
    {
        ThisRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OnMouseDown)
        {
            NowMousePosition = Input.mousePosition;

            ThisRect.localPosition = new Vector2(ThisRect.localPosition.x-((MotoMousePosition.x-NowMousePosition.x)*1.3f),0);

            if (ThisRect.localPosition.x>=1080)
            {
                ThisRect.localPosition = new Vector2(1080,0);
            }

            if (ThisRect.localPosition.x<=-1080)
            {
                ThisRect.localPosition = new Vector2(-1080,0);
            }

            MotoMousePosition = Input.mousePosition;
        }
    }

    public void MouseDown()
    {
        OnMouseDown = true;

        MotoMousePosition = Input.mousePosition;
    }

    public void MouseUp()
    {
        OnMouseDown = false;
    }
}
