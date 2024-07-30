using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimeScript : MonoBehaviour
{
    private GameObject CanvasObject;
    public GameObject TitleManeger,VersionText;

    public GameObject PressStartPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CanvasObject = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void MakePressStart()
    {

        if (!GameObject.Find("PressStart(Clone)"))
        {

            Instantiate(PressStartPrefab, CanvasObject.transform);
        }

        TitleManeger.GetComponent<TitleManeger>().WaitPressFlag = true;

        if (!AllAudioManege.BGMAudio.isPlaying)
        {
            AllAudioManege.PlayBGM(0);
        }
       
        VersionText.GetComponent<Text>().text = "Ver " + Application.version;
    }

}
