using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace STGGame.Item
{
    public class ItemBase : MonoBehaviour
    {
    #region 定義

    

    #endregion
    
    #region 変数

    #region SerializeField

        /// <summary>
        /// リジッドボディ
        /// </summary>
        [SerializeField]
        protected Rigidbody2D rigidBody;

        /// <summary>
        /// スポーン時にかかる力
        /// </summary>
        [SerializeField]
        protected float spawnAddForce;
        
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

        /// <summary>
        /// スポーン時の挙動
        /// </summary>
        protected void Spawn()
        {
            rigidBody.AddForce(Vector2.up * spawnAddForce, ForceMode2D.Impulse);
        }

    #endregion
    
    #region UnityMethod

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            Spawn();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {

        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
            
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag(TagDefine.ITEM_DEAD_ZONE))
            {
                Destroy(this.gameObject);
            }
        }

    #endregion

    #endregion
    }
}