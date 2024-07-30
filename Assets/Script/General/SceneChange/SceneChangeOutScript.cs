using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOutScript : MonoBehaviour
{

    private GameObject ThisParent;
    private Rigidbody2D ThisRigid;

    public int ThisSceneChangeType;
    public int TokusyuType;

    // Start is called before the first frame update
    void Start()
    {
        ThisRigid = GetComponent<Rigidbody2D>();

        ThisParent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        //右へ
        if (ThisSceneChangeType == 1)
        {
            ThisRigid.AddForce(new Vector2(2.5f, 0));

            if (GetComponent<RectTransform>().localPosition.x >= 650)
            {
                if (TokusyuType==1)
                {
                    DangeonManeger.CanCommandFlag = true;
                    DangeonManeger.CanMesFlag = true;
                }

                Destroy(ThisParent);
            }
        }

        //左へ
        if (ThisSceneChangeType == 2)
        {
            ThisRigid.AddForce(new Vector2(-2.5f, 0));

            if (GetComponent<RectTransform>().localPosition.x<=-650)
            {
                if (TokusyuType == 1)
                {
                    DangeonManeger.CanCommandFlag = true;
                    DangeonManeger.CanMesFlag = true;
                }

                Destroy(ThisParent);
            }
        }
    }
}
