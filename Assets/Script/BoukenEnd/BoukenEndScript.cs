using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoukenEndScript : MonoBehaviour
{
    public GameObject SceneChangePrefab,KoukokuPrefab;
    private GameObject SceneChange,KoukokuSentaku;

    public GameObject KoukokuButton;

    private GameObject[] DeleteObject = new GameObject[4];

    private Text BoukenEndText;

    private string[] HyoujiText=new string[2];
    
    public static bool MesEndFlag,CanMesFlag,CanSinkouFlag,NibaiFlag,GetChangeFlag;

    private int NowMesNum, NowGyouNum;

    public float TextTime;
    private float TextKeikaTime;

    // Start is called before the first frame update
    void Start()
    {

        BoukenEndText = gameObject.GetComponent<Text>();

        DeleteObject[0] = GameObject.Find("AllDataManege");
        DeleteObject[1] = GameObject.Find("AudioManeger");
        DeleteObject[2] = GameObject.Find("IAPListener");
        DeleteObject[3] = GameObject.Find("AdsListener");

        HyoujiText[0] = "あなたの冒険は終わりました";

        //AllDataManege.CanGetKeiken = false;//後で消す
        //AllDataManege.CharactorData.KihonParameter[0] = 20;//後で消す

        //バトル敗北の場合は広告でゲットできるようにする
        if (!AllDataManege.CanGetKeiken)
        {
            GetChangeFlag = true;
        }

        //もしも旅の経験を入手できなかったら二倍ボタンを消す
        if (AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0] - 1) == 0)
        {
            Destroy(KoukokuButton);
        }

        CanMesFlag = true;

        StartCoroutine("Wait");
    }

    // Update is called once per frame
    void Update()
    {
        //rayを作成
        //メインカメラ上のマウスカーソルのある位置からRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //レイヤーマスク作成
        int layerMask = LayerMask.NameToLayer("DEFAULT");

        //Rayの長さ
        float maxDistance = 10;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance, layerMask);

        //進行フラグがたってないと進まない
        if (!CanSinkouFlag)
        {
            return;
        }

        //文字を追加していく
        if (!MesEndFlag&&CanMesFlag)
        {
            if (TextKeikaTime>=TextTime)
            {
                BoukenEndText.text += HyoujiText[NowGyouNum][NowMesNum];

                NowMesNum++;

                if (NowMesNum == HyoujiText[NowGyouNum].Length)
                {
                    MesEndFlag = true;

                    return;
                }

                TextKeikaTime = 0;
            }

            TextKeikaTime += Time.deltaTime;
        }

        //文字の行を進める
        if (MesEndFlag&&CanMesFlag&&Input.GetMouseButtonDown(0)&&!hit.collider)
        {
            if (NowGyouNum==0)
            {
                Destroy(KoukokuButton);

                //もし旅の経験をゲットできたら
                if (AllDataManege.CanGetKeiken)
                {
                    BoukenEndText.text = "";

                    NowGyouNum++;
                    NowMesNum = 0;

                    MesEndFlag = false;

                    //旅の経験を手に入れられなかったら
                    if (AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0] - 1) == 0)
                    {
                        HyoujiText[1] = "旅の経験を手に入れられなかった……";
                    }
                    //手に入れていたら
                    else
                    {
                        //二倍になっていなかったら
                        if (!NibaiFlag)
                        {
                            HyoujiText[1] = "旅の経験を" + AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0]) + "個手に入れた！";

                        }
                        //なっていたら
                        else
                        {
                            HyoujiText[1] = "旅の経験を" + AllDataManege.SomeIntData.GetGetTabikeiken(AllDataManege.CharactorData.KihonParameter[0])*2 + "個手に入れた！";

                        }
                    }
                }
                //できなかったら
                else
                {
                    SceneChange = Instantiate(SceneChangePrefab, gameObject.transform.parent.transform);
                }

                return;
            }

            if (NowGyouNum==1)
            {
                SceneChange = Instantiate(SceneChangePrefab,gameObject.transform.parent.transform);

                CanMesFlag = false;
            }
        }

        //タイトルへ遷移
        if (SceneChange&&SceneChange.GetComponent<Image>().color.a==1f)
        {
            SceneChange.GetComponent<Animation>().enabled = false;

            Destroy(DeleteObject[0]);
            Destroy(DeleteObject[1]);
            Destroy(DeleteObject[2]);
            Destroy(DeleteObject[3]);

            SceneManager.LoadScene("Title");
        }
    }

    //広告を表示するかどうかのボタンを表示
    public void MakeKoukokuSentaku()
    {
        if (!CanSinkouFlag)
        {
            return;
        }

        KoukokuSentaku = Instantiate(KoukokuPrefab,gameObject.transform.parent.transform);
        KoukokuSentaku.GetComponent<EndKoukokuScript>().BoukenEndScript = this;

        CanSinkouFlag = false;
        CanMesFlag = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);

        CanSinkouFlag = true;

        if (KoukokuButton)
        {
            KoukokuButton.GetComponent<ImageInOutScript>().InFlag = true;
        }

    }
}
