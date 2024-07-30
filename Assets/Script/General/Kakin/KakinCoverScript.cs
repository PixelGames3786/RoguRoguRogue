using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class KakinCoverScript : MonoBehaviour
{
    [System.NonSerialized]
    public Text LoadingText;

    [System.NonSerialized]
    public bool TextFlag;

    private int TextType;

    private float KeikaTime;
    public float TextTime;

    // Start is called before the first frame update
    void Start()
    {
        TextFlag = true;

        LoadingText = gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TextFlag)
        {
            KeikaTime += Time.unscaledDeltaTime;

            if (KeikaTime>TextTime)
            {
                KeikaTime = 0;

                if (TextType==0)
                {
                    LoadingText.text = "少々お待ちください.";

                    TextType++;

                    return;
                }

                if (TextType==1)
                {
                    LoadingText.text = "少々お待ちください..";

                    TextType++;

                    return;
                }

                if (TextType==2)
                {
                    LoadingText.text = "少々お待ちください...";

                    TextType++;

                    return;
                }

                if (TextType==3)
                {
                    LoadingText.text = "少々お待ちください";

                    TextType = 0;

                    return;
                }
            }
        }
    }

    //押されたとき
    public void GazouDown()
    {
        if (!TextFlag)
        {
            Destroy(gameObject);

            Time.timeScale = 1;
        }
    }
}
