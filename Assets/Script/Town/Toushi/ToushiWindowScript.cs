using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToushiWindowScript : MonoBehaviour
{
    public AnimationClip OutAnimation;

    public GameObject KakuninPrefab;
    private GameObject NowSentakuButton,KakuninWindow;

    private Text ToushiText;
    private Text SyojikinText, ToushikinText;

    private bool OnTouthFlag;

    public int ToushiKingaku,ToushiKeta;

    public int[] ToushiKingakuKari,ToushiKingakuSuji = new int[4];
    //0 一のけた 1 十のけた 2 百のけた 3 千のけた

    // Start is called before the first frame update
    void Start()
    {
        ToushiText = gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

        SyojikinText = gameObject.transform.GetChild(3).GetComponent<Text>();
        ToushikinText = gameObject.transform.GetChild(4).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animation>().clip.name == "ToushiWindowOut" && !this.GetComponent<Animation>().isPlaying)
        {
            TownManegerScript.CanCommandFlag = true;
            TownManegerScript.ToushiHantei();

            Destroy(this.gameObject);
        }

        //所持金と投資金額のテキスト更新
        SyojikinText.text = "所持金：" + AllDataManege.CharactorData.GetSyojikin();
        ToushikinText.text = "投資金額：" + AllDataManege.TownData.GetToushiMoney();

        ToushiText.text = ToushiKingaku.ToString();
    }

    public void ButtonDown(GameObject Button)
    {
        if (KakuninWindow)
        {
            return;
        }

        Button.GetComponent<Image>().color = new Color(0.8f,0.8f,0.8f);

        Button.transform.GetChild(0).GetComponent<Text>().color = new Color(0.2f,0.2f,0.2f);

        OnTouthFlag = true;

        NowSentakuButton = Button;
        
    }

    public void ButtonUp(GameObject Button)
    {
        if (KakuninWindow)
        {
            return;
        }

        Button.GetComponent<Image>().color = new Color(1f, 1f, 1f);

        Button.transform.GetChild(0).GetComponent<Text>().color = new Color(0f, 0f, 0f);

        OnTouthFlag = false;

    }

    public void TriggerOut(GameObject Button)
    {
        if (KakuninWindow)
        {
            return;
        }

        if (Button==NowSentakuButton)
        {
            Button.GetComponent<Image>().color = new Color(1f, 1f, 1f);

            Button.transform.GetChild(0).GetComponent<Text>().color = new Color(0f, 0f, 0f);

            OnTouthFlag = false;
        }
    }

    //数字のボタンが押されたとき
    public void NumButtonDown(int value)
    {
        //ボタンの色変更
        NowSentakuButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);

        NowSentakuButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0f, 0f, 0f);

        //もう桁がマックスの時
        if (!OnTouthFlag||KakuninWindow||ToushiKeta==4)
        {
            return;
        }

        //もし０の場合は一個繰り上げる
        if (value==0&&ToushiKeta>0)
        {
            for (int i=ToushiKeta;i>0;i--)
            {
                ToushiKingakuKari[i] = ToushiKingakuKari[i - 1];
            }

            ToushiKingakuKari[0] = 0;
        }
        //0の時には0を押しても意味はない
        else if(value==0&&ToushiKeta==0)
        {
            OnTouthFlag = false;

            return;
        }
        //0以外の数字が押されたとき
        else
        {
            for (int i=0;i<ToushiKeta;i++)
            {
                ToushiKingakuKari[i + 1] = ToushiKingakuSuji[i];
            }

            //投資金額の計算
            ToushiKingakuKari[0] = value;
        }

        ToushiKeta++;

        ToushiKingaku = ToushiKingakuKari[0] + ToushiKingakuKari[1] * 10 + ToushiKingakuKari[2] * 100 + ToushiKingakuKari[3] * 1000;

        for (int i=0;i<4;i++)
        {
            ToushiKingakuSuji[i] = ToushiKingakuKari[i];
        }

        OnTouthFlag = false;
    }

    //他機能のボタンが押されたとき
    public void SubButtonDown(int value)
    {
        //ボタンの色変更
        NowSentakuButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);

        NowSentakuButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0f, 0f, 0f);

        //確認ウィンドウがないもしくはボタンの上にポインターがない
        if (!OnTouthFlag||KakuninWindow)
        {
            return;
        }

        //押されたボタンによって処理を変える
        switch (value)
        {
            //投資ボタン
            case 1:

                {
                    //所持金が足りない場合
                    if (ToushiKingaku > AllDataManege.CharactorData.GetSyojikin())
                    {
                        KakuninWindow = Instantiate(KakuninPrefab, gameObject.transform.parent.transform);

                        KakuninWindow.transform.GetChild(0).GetComponent<Text>().text = "所持金が足りないよ！";

                        return;
                    }

                    //所持金が足りている場合

                    AllDataManege.TownData.SetToushiMoney(AllDataManege.TownData.GetToushiMoney() + ToushiKingaku);
                    AllDataManege.CharactorData.SetSyojikin(AllDataManege.CharactorData.GetSyojikin() - ToushiKingaku);

                    GetComponent<Animation>().clip = OutAnimation;

                    GetComponent<Animation>().Play();

                    AllAudioManege.PlaySE(6);

                    AllDataManege.DataSave();
                }

                break;

            //戻るボタン
            case 2:

                {
                    GetComponent<Animation>().clip = OutAnimation;

                    GetComponent<Animation>().Play();

                    AllAudioManege.PlaySE(6);
                }

                break;

            //一つ消しボタン
            case 3:

                {
                    //もし一桁以上だったら
                    if (ToushiKeta > 0)
                    {
                        ToushiKeta--;

                        for (int i = 1; i < 4; i++)
                        {
                            ToushiKingakuKari[i - 1] = ToushiKingakuSuji[i];
                        }

                        ToushiKingakuKari[ToushiKeta] = 0;
                    }

                    ToushiKingaku = ToushiKingakuKari[0] + ToushiKingakuKari[1] * 10 + ToushiKingakuKari[2] * 100 + ToushiKingakuKari[3] * 1000;

                    for (int i = 0; i < 4; i++)
                    {
                        ToushiKingakuSuji[i] = ToushiKingakuKari[i];
                    }
                }

                break;

            //全消しボタン
            case 4:

                {
                    ToushiKeta = 0;

                    ToushiKingakuKari[0] = 0;
                    ToushiKingakuKari[1] = 0;
                    ToushiKingakuKari[2] = 0;
                    ToushiKingakuKari[3] = 0;

                    ToushiKingaku = 0;
                }

                break;
        }

        OnTouthFlag = false;
    }
}
