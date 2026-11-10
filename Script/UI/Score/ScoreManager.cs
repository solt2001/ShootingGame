using TMPro;
using UnityEngine;

namespace STGGame.Score
{
    public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
    {
    #region 定義

    

    #endregion
    
    #region 変数

    #region SerializeField

        /// <summary>
        /// スコアテキスト
        /// </summary>
        [Header("ゲームインフォUI関連")]
        [SerializeField]
        protected TMP_Text scoreNumText;

    #endregion

    #region Public

        

    #endregion

    #region Private

        protected override bool dontDestroyOnLoad { get; }
    
        /// <summary>
        /// グローバルスコア
        /// </summary>
        protected long globalScore = 0;

        public long GlobalScore
        {
            get { return globalScore; }
        }

    #endregion
    
    #endregion
    
    #region Method

    #region Public

        /// <summary>
        /// スコアをテキストに反映
        /// </summary>
        /// <param name="score"></param>
        /// <param name="isScoreApply">スコアをグローバルスコアとして反映するか</param>
        public void SetScoreText(long score, bool isScoreApply = false)
        {
            if (isScoreApply)
            {
                globalScore = score;
            }
        
            scoreNumText.text = score.ToString();
        }

        /// <summary>
        /// スコア加算（テキスト反映まで）
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(long score)
        {
            globalScore += score;
        
            SetScoreText(globalScore);
        }

        /// <summary>
        /// スコア初期化
        /// </summary>
        public virtual void InitScore()
        {
            SetScoreText(0, true);
        }

    #endregion

    #region Private

        

    #endregion
    
    #region UnityMethod

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // スコア初期化
            InitScore();
        }

    #endregion

    #endregion
    }
}