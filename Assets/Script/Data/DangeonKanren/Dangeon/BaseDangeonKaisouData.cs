using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "DangeonData", menuName = "CreateData/CreateDangeon/DangeonKaisouData")]
public class BaseDangeonKaisouData : ScriptableObject
{

    //一回先に進むときに増えるパーセント
    [SerializeField]
    public int SinkouParcent;

    //ダンジョンのスプライト
    [SerializeField]
    public Sprite DangeonSprite;

    //イベントが発生する確率
    [SerializeField]
    public int[] IventKakuritu;

    //発生するイベント
    [SerializeField]
    public int[] HasseiIvent;
    //0 何も起こらない 1 敵 2 宝箱

    //敵ごとの確率
    [SerializeField]
    public int[] EnemyKakuritu;

    //遭遇する敵
    [SerializeField]
    public int[] SouguEnemy;

    //宝箱から出てくるアイテムの確率
    [SerializeField]
    public int[] TakarabakoKakuritu;

    //宝箱から出てくるアイテム
    [SerializeField]
    public int[] TakarabakoItem;

    //宝箱が罠だった場合に与えるダメージ
    [SerializeField]
    public int[] TakarabakoDamege=new int[2];

    //宝箱がＧだった場合にもらえるゴールド
    [SerializeField]
    public int[] TakarabakoGold=new int[2];

    //一休みするときに回復する数値
    [SerializeField]
    public int[] HitoyasumiKaihuku=new int[2];

    //行商人のデータ
    [SerializeField]
    public BaseGyousyouData GyousyouData;

    //その階層のボス
    [SerializeField]
    public BaseEnemySyuzokuData BossData;

    //テキストの行数最低
    [SerializeField]
    public int TextGyousuuMin;

    //テキストの行数上限
    [SerializeField]
    public int TextGyousuuMax;

    //その階のＢＧＭ
    [SerializeField]
    public int DangeonBGM;
}
