using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressStart : MonoBehaviour
{

    public float Speed;

    private bool InOutFlag;

    // Start is called before the first frame update
    void Start()
    {
        InOutFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InOutFlag)
        {
            this.GetComponent<Text>().color = new Color(0, 0, 0, this.GetComponent<Text>().color.a - Speed*Time.deltaTime);

            if (this.GetComponent<Text>().color.a <= 0f)
            {
                InOutFlag = false;
            }
        }
        else
        {
            this.GetComponent<Text>().color = new Color(0,0,0, this.GetComponent<Text>().color.a+ Speed * Time.deltaTime);

            if (this.GetComponent<Text>().color.a>=1f)
            {
                InOutFlag = true;
            }
        }
    }
}
