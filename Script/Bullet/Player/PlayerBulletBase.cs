using UnityEngine;

namespace STGGame.Bullet.Player
{
    public class PlayerBulletBase : BulletBase
    {
    #region 定義

    

    #endregion
    
    #region 変数

    #region SerializeField

        /// <summary>
        /// 攻撃力
        /// </summary>
        [SerializeField]
        protected float bulletAttack;

        /// <summary>
        /// 弾の速度
        /// </summary>
        [SerializeField]
        protected float bulletSpeed;
        
    #endregion

    #region Public

        /// <summary>
        /// 攻撃力
        /// </summary>
        public float BulletAttack => bulletAttack;

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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        protected override void Update()
        {
            targetTransform.Translate(0, bulletSpeed, 0);
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(TagDefine.ENEMY))
            {
                Destroy(this.gameObject);
            }
        }

        protected  void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(TagDefine.ENEMY))
            {
                Destroy(this.gameObject);
            }
        }

    #endregion

    #endregion
    }
}