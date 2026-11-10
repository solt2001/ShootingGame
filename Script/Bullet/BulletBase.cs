using System;
using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using UnityEngine;

namespace STGGame.Bullet
{
    /// <summary>
    /// 弾基底クラス
    /// </summary>
    public class BulletBase : MonoBehaviour
    {
    #region 定義

        

    #endregion
        
    #region 変数

        #region SerializeField

        /// <summary>
        /// トランスフォーム
        /// </summary>
        [SerializeField]
        protected Transform targetTransform;

        #endregion

        #region Public



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

        protected virtual void Start()
        {
            
        }

        protected virtual void Update()
        {
            
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            //　デッドゾーンにあたったら自身を消す
            if (other.gameObject.CompareTag(TagDefine.BULLET_DEAD_ZONE))
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            //　デッドゾーンにあたったら自身を消す
            if (other.gameObject.CompareTag(TagDefine.BULLET_DEAD_ZONE))
            {
                Destroy(this.gameObject);
            }
        }

    #endregion

    #endregion
    }
}
