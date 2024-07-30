using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeInScript : MonoBehaviour
{

    private Rigidbody2D ThisRigid;

    public int ThisSceneChangeType;
    private bool ThisIdouFlag;

    // Start is called before the first frame update
    void Start()
    {
        ThisRigid = this.GetComponent<Rigidbody2D>();

        ThisIdouFlag = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (ThisIdouFlag)
        {
            if (ThisRigid.IsSleeping())
            { 
                ThisIdouFlag = false;
            }

            //左へ
            if (ThisSceneChangeType==1)
            {
                ThisRigid.AddForce(new Vector2(-2.5f, 0));
            }

            //右へ
            if (ThisSceneChangeType == 2)
            {
                ThisRigid.AddForce(new Vector2(2.5f, 0));
            }
        }

    }

    //ぶつかったとき
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ThisIdouFlag = false;
    }
}
