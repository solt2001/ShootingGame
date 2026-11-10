using TMPro;
using UnityEngine;

namespace STGGame.PlayerInfo
{
    /// <summary>
    /// プレイヤー
    /// </summary>
    public class PlayerInfoManager : SingletonMonoBehaviour<PlayerInfoManager>
    {
    #region 定義

    

    #endregion
    
    #region 変数

    #region SerializeField

        /// <summary>
        /// パワー数値テキスト
        /// </summary>
        [SerializeField]
        private TMP_Text powerNumText;

        /// <summary>
        /// 残ボム数テキスト
        /// </summary>
        [SerializeField] 
        private TMP_Text bombNumText;
        
    #endregion

    #region Public

        

    #endregion

    #region Private

        protected override bool dontDestroyOnLoad { get; }

    #endregion
    
    #endregion
    
    #region Method

    #region Public
    
        /// <summary>
        /// パワーテキストセット
        /// </summary>
        /// <param name="powerNum"></param>
        public void SetPowerNumText(float powerNum)
        {
            powerNumText.text = powerNum.ToString();
        }

        public void SetBombNumText(int bombNum)
        {
            bombNumText.text = bombNum.ToString();
        }

    #endregion

    #region Private

        

    #endregion
    
    #region UnityMethod



    #endregion

    #endregion
    }
}
