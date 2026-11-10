using UnityEngine;

namespace STGGame.PlayerInfo
{
    public static class PlayerInfoFunction
    {
    #region 定義

    

    #endregion

    #region Method

    #region Public

        /// <summary>
        /// パワーをテキストに反映
        /// </summary>
        /// <param name="power"></param>
        public static void SetPowerText(float power)
        {
            PlayerInfoManager.Instance.SetPowerNumText(power);
        }

        /// <summary>
        /// ボム数をテキストに反映
        /// </summary>
        /// <param name="bombNum"></param>
        public static void SetBombNum(int bombNum)
        {
            PlayerInfoManager.Instance.SetBombNumText(bombNum);
        }

    #endregion

    #region Private

        

    #endregion

    #endregion
    }
}