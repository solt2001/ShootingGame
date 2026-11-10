using STGGame.Controller;
using UnityEngine;

namespace STGGame.Score
{public static class ScoreFunction
    {
    #region 定義

    

    #endregion
    
    #region Method

    #region Public

        /// <summary>
        /// スコア初期化
        /// </summary>
        public static void InitScore()
        {
            ScoreManager.Instance.InitScore();
        }
        
        /// <summary>
        /// スコアテキスト反映
        /// </summary>
        /// <param name="score"></param>
        /// <param name="isScoreApply">スコアをグローバルスコアに反映させるか（加算ではなく直反映）</param>
        public static void SetScore(int score, bool isScoreApply)
        {
            ScoreManager.Instance.SetScoreText(score, isScoreApply);
        }

        /// <summary>
        /// スコア加算
        /// </summary>
        /// <param name="score"></param>
        public static void AddScore(long score)
        {
            ScoreManager.Instance.AddScore(score);
        }

    #endregion

    #region Private

        

    #endregion
        
    #endregion
    }
}