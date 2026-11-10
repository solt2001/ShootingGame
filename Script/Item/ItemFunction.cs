using System.Collections.Generic;
using STGGame.Enemy;
using UnityEngine;

public static class ItemFunction
{
#region 定義

    /// <summary>
    /// 抽選確率の最大値
    /// </summary>
    public const int LOTTERY_CHANCE_MAX = 99;
    
    /// <summary>
    /// 抽選確率の最小値
    /// </summary>
    public const int LOTTERY_CHANCE_MIN = 0;

#endregion
    
#region 変数

    #region Public

        

    #endregion

    #region Private

        

    #endregion
    
#endregion
    
#region Method

    #region Public

    public static int LotteryItem(List<EnemyBase.LotteryItem> lotteryItemList)
    {
        // 100分率で抽選
        int lottery = Random.Range(LOTTERY_CHANCE_MIN, LOTTERY_CHANCE_MAX);

        for (int i = 0; i < lotteryItemList.Count; i++)
        {
            var item = lotteryItemList[i];
            if (lottery <= item.lotteryChance)
            {
                return i;
            }
        }

        return -1;
    }

    #endregion

    #region Private

        

    #endregion

#endregion
}