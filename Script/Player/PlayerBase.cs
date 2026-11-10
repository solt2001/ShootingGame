using System;
using UnityEngine;
using STGGame.InputController;
using STGGame.Item;
using STGGame.Score;

namespace STGGame.Player
{
    /// <summary>
    /// プレイヤーの性能、見た目を制御する
    /// </summary>
    public class PlayerBase : MonoBehaviour
    {
#region 定義

        [Serializable]
        public class Param
        {
            /// <summary>
            /// パワー
            /// </summary>
            public float power;
            
            /// <summary>
            /// ボム
            /// </summary>
            public int bomb;
        }

#endregion
    
    #region 変数

        #region SerializeField
        
        /// <summary>
        /// 表示するスプライト
        /// </summary>
        [SerializeField]
        protected SpriteRenderer sprite;
        
        /// <summary>
        /// プレイヤー操作コンポーネント
        /// </summary>
        [SerializeField]
        protected PlayerControllerBase playerController;
        
        /// <summary>
        /// 動くスピード
        /// </summary>
        [SerializeField]
        protected float moveSpeed;

        /// <summary>
        /// スロー移動倍率
        /// </summary>
        [SerializeField]
        protected float slowMag;
        
        /// <summary>
        /// 弾を打つ間隔
        /// </summary>
        [SerializeField]
        protected float bulletInterval;
        
        /// <summary>
        /// 弾Prefab
        /// </summary>
        [SerializeField]
        protected GameObject bulletPrefab;
        
        #endregion

        #region Public
        
        /// <summary>
        /// 弾Prefab
        /// </summary>
        public GameObject BulletPrefab => bulletPrefab;

        #endregion

        #region Private
        
        /// <summary>
        /// スプライトのバウンス
        /// </summary>
        public Bounds PlayerSpriteBounds
        {
            get { return sprite.bounds; }
        }
                
        #endregion
        
    #endregion
        
    #region Method

        #region Public
            
            
                
        #endregion

        #region Private

                
                
        #endregion
                
        #region UnityMethod

        protected void Awake()
        {
            playerController.Setting(moveSpeed, slowMag, bulletInterval);
        }
        
        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="collision"></param>
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            // エネミー弾に当たったら、当たった時の処理を走らせる
            if (collision.gameObject.CompareTag(TagDefine.ENEMY_BULLET))
            {
                //Debug.Log("ヒットした！");
            }
            else if (collision.gameObject.CompareTag(TagDefine.BLOCK_WALL))
            {
                playerController.SetIsMoveClamp(true);
            }
            else if (collision.gameObject.CompareTag(TagDefine.POWER_ITEM))
            {
                
            }
        }

        protected void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(TagDefine.BLOCK_WALL))
            {
                playerController.SetIsMoveClamp(true);
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(TagDefine.BLOCK_WALL))
            {
                playerController.SetIsMoveClamp(false);
            }
        }

    #endregion

    #endregion
    }
}

