using System;
using STGGame.Item;
using STGGame.Score;
using UnityEngine;

namespace STGGame.Item
{
    public class ScoreItem : ItemBase
    {
    #region 定義

    

    #endregion
    
    #region 変数

    #region SerializeField

        [SerializeField]
        private int score;

    #endregion

    #region Public

        public int Score
        {
            get { return score; }
        }

    #endregion

    #region Private

        

    #endregion
    
    #endregion

    #region Method

    #region Public

    

    #endregion

    #region Private

        

    #endregion
    
    #region UnityMethod

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            // ヒットボックスに当たったらスコア加算
            if (collision.collider.gameObject.CompareTag(TagDefine.HIT_BOX))
            {
                ScoreFunction.AddScore(score);
                Destroy(this.gameObject);
            }
        }

    #endregion

    #endregion
    }
}
