using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ImageInOutScript : MonoBehaviour
{
    private Image ThisImage;
    private Text ThisText;

    [System.NonSerialized]
    public bool InFlag, OutFlag;
    
    public int TokusyuType;
    //0 image 1 text

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        if (TokusyuType==0)
        {
            ThisImage = gameObject.GetComponent<Image>();

            InFlag = true;

        }
        else if (TokusyuType==1)
        {

            ThisText = gameObject.GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {

            if (TokusyuType==0)
            {
                ThisImage.color = new Color(1, 1, 1, ThisImage.color.a + (Speed * Time.deltaTime));

                if (ThisImage.color.a >= 1.0f)
                {
                    InFlag = false;
                }
            }
            else if (TokusyuType==1)
            {
                ThisText.color = new Color(1, 1, 1, ThisText.color.a + (Speed * Time.deltaTime));

                if (ThisText.color.a >= 1.0f)
                {
                    InFlag = false;
                }
            }

        }

        if (OutFlag)
        {

            if (TokusyuType==0)
            {
                ThisImage.color = new Color(1, 1, 1, ThisImage.color.a - (Speed * Time.deltaTime));

                if (ThisImage.color.a <= 0f)
                {
                    OutFlag = false;

                    Destroy(gameObject);
                }
            }
            else if (TokusyuType==1)
            {
                ThisText.color = new Color(1, 1, 1, ThisText.color.a + (Speed * Time.deltaTime));

                if (ThisText.color.a <=0f)
                {
                    OutFlag = false;

                    Destroy(gameObject);
                }
            }
        }
    }
}
