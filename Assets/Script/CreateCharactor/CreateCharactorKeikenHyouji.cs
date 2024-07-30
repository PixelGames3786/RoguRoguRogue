using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharactorKeikenHyouji : MonoBehaviour
{

    private Text ThisText;

    // Start is called before the first frame update
    void Start()
    {
        ThisText=this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ThisText.text = "旅の経験：" + AllDataManege.SomethingData.GetTabiKeiken();
    }
}
