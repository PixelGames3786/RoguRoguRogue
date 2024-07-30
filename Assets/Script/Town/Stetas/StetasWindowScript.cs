using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StetasWindowScript : MonoBehaviour
{
    public GameObject ButtonWindowPrefab,ButtonWindowPrefab2,ButtonWindowPrefab3;

    [System.NonSerialized]
    public GameObject ButtonWindow,ButtonWindow2,ButtonWindow3,ItemSetumeiWindow;

    public AnimationClip OutAnimation;

    private Transform StetasTrans;

    private int[] HyoujiParameter = new int[15];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animation>().clip.name=="StetasWindowOut"&&!this.GetComponent<Animation>().isPlaying)
        {
            TownManegerScript.CanCommandFlag = true;

            Destroy(this.gameObject);
        }

        StetasHyouji();
    }

    public void BackButtonDown()
    {
            this.GetComponent<Animation>().clip = OutAnimation;

            this.GetComponent<Animation>().Play();

            AllAudioManege.PlaySE(6);
    }

    public void SoubiButtonDown()
    {

        if (ButtonWindow3)
        {
            AllAudioManege.PlaySE(4);

            ButtonWindow = Instantiate(ButtonWindowPrefab, this.gameObject.transform);

            ButtonWindow3.GetComponent<ButtonWindowScript>().OutFlag = true;

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (ButtonWindow2)
        {
            AllAudioManege.PlaySE(4);

            ButtonWindow = Instantiate(ButtonWindowPrefab, this.gameObject.transform);

            ButtonWindow2.GetComponent<ButtonWindowScript>().OutFlag = true;

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (ButtonWindow)
        {
            AllAudioManege.PlaySE(6);

            ButtonWindow.GetComponent<ButtonWindowScript>().OutFlag = true;

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (!ButtonWindow)
        {
            AllAudioManege.PlaySE(4);

            ButtonWindow = Instantiate(ButtonWindowPrefab, this.gameObject.transform);
        }
    }

    public void BagButtonDown()
    {

        if (ButtonWindow)
        {
            AllAudioManege.PlaySE(3);

            ButtonWindow.GetComponent<ButtonWindowScript>().OutFlag = true;

            ButtonWindow2 = Instantiate(ButtonWindowPrefab2, this.gameObject.transform);

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (ButtonWindow2)
        {
            AllAudioManege.PlaySE(6);

            ButtonWindow2.GetComponent<ButtonWindowScript>().OutFlag = true;

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (ButtonWindow3)
        {
            AllAudioManege.PlaySE(3);

            ButtonWindow3.GetComponent<ButtonWindowScript>().OutFlag = true;

            ButtonWindow2 = Instantiate(ButtonWindowPrefab2, this.gameObject.transform);

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (!ButtonWindow2)
        {
            AllAudioManege.PlaySE(3);

            ButtonWindow2 = Instantiate(ButtonWindowPrefab2,this.gameObject.transform);
        }
    }

    public void SkillButtonDown()
    {
        if (ButtonWindow)
        {
            AllAudioManege.PlaySE(5);

            ButtonWindow.GetComponent<ButtonWindowScript>().OutFlag = true;

            ButtonWindow3 = Instantiate(ButtonWindowPrefab3, this.gameObject.transform);

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (ButtonWindow2)
        {
            AllAudioManege.PlaySE(5);

            ButtonWindow2.GetComponent<ButtonWindowScript>().OutFlag = true;

            ButtonWindow3 = Instantiate(ButtonWindowPrefab3, this.gameObject.transform);

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (ButtonWindow3)
        {
            AllAudioManege.PlaySE(6);

            ButtonWindow3.GetComponent<ButtonWindowScript>().OutFlag = true;

            if (ItemSetumeiWindow)
            {
                ItemSetumeiWindow.GetComponent<StetasSetumeiScript>().OutFlag = true;
            }

            return;
        }

        if (!ButtonWindow3)
        {
            AllAudioManege.PlaySE(5);

            ButtonWindow3 = Instantiate(ButtonWindowPrefab3, this.gameObject.transform);
        }
    }

    private void StetasHyouji()
    {
        HyoujiParameter = AllDataManege.HyoujiParameterReset();

        StetasTrans = transform.GetChild(1).transform;

        StetasTrans.GetChild(0).GetComponent<Text>().text = "HP " + HyoujiParameter[2] + "/" + HyoujiParameter[1]; //HP表示
        StetasTrans.GetChild(1).GetComponent<Text>().text = "MP " + HyoujiParameter[4] + "/" + HyoujiParameter[3]; //MP表示
        StetasTrans.GetChild(2).GetComponent<Text>().text = "ATK  " + HyoujiParameter[5];
        StetasTrans.GetChild(3).GetComponent<Text>().text = "DEF  " + HyoujiParameter[6];
        StetasTrans.GetChild(4).GetComponent<Text>().text = "INT  " + HyoujiParameter[7];
        StetasTrans.GetChild(5).GetComponent<Text>().text = "MIN  " + HyoujiParameter[8];
        StetasTrans.GetChild(6).GetComponent<Text>().text = "DEX  " + HyoujiParameter[9];
        StetasTrans.GetChild(7).GetComponent<Text>().text = "AGI  " + HyoujiParameter[10];
        StetasTrans.GetChild(8).GetComponent<Text>().text = "SPD  " + HyoujiParameter[11];
        StetasTrans.GetChild(9).GetComponent<Text>().text = "LUC  " + HyoujiParameter[12];
        StetasTrans.GetChild(10).GetComponent<Text>().text = "CHR  " + HyoujiParameter[13];

        StetasTrans.GetChild(11).GetComponent<Text>().text = "LV  " + HyoujiParameter[0];
        StetasTrans.GetChild(12).GetComponent<Text>().text = HyoujiParameter[14] + "G";
        StetasTrans.GetChild(13).GetComponent<Text>().text = "<size=80>EXP</size> " + HyoujiParameter[15];
    }
}
