using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownSyojikinManeger : MonoBehaviour
{
    private RectTransform ThisRect;

    private Text ThisTextMesh;

    private bool InFlag;

    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        InFlag = true;

        ThisRect = this.gameObject.GetComponent<RectTransform>();
        ThisTextMesh = this.gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InFlag)
        {
            ThisRect.localPosition = new Vector3(ThisRect.localPosition.x+(Speed*Time.deltaTime),790,0);

            ThisRect.localPosition = new Vector3(Mathf.Clamp(ThisRect.localPosition.x,-720,-365),790,0);

            if (ThisRect.localPosition.x>=-365)
            {
                InFlag = false;
            }
        }

        ThisTextMesh.text = AllDataManege.CharactorData.GetSyojikin() + "G";
    }
}
