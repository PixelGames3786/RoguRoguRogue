using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseTownData 
{
    
    public int HattenLevel;
    
    public int ToushiMoney;
    
    public int[] CommandKaihou=new int[8];
    //0 村長の家 1 宿屋 2 路地裏 3 戻る 4 道具屋 5 鍛冶屋 6 訓練場 7 図書館
    
    public List<int> DouguyaItemInt=new List<int>();
    public List<int> BukiyaItemInt=new List<int>();
    public List<int> BouguyaItemInt=new List<int>();

    //ゲッター
    public int GetHattenLevel()
    {
        return HattenLevel;
    }

    public int GetToushiMoney()
    {
        return ToushiMoney;
    }

    public int[] GetCommandKaihou()
    {
        return CommandKaihou;
    }

    //セッター

    public void SetHattenLevel(int value)
    {
        HattenLevel = value;
    }

    public void SetToushiMoney(int value)
    {
        ToushiMoney = value;
    }

    public void SetCommandKaihou(int[] value)
    {
        CommandKaihou = value;
    }
}
