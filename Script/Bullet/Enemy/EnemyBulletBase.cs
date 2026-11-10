using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace STGGame.Bullet
{
    public class EnemyBulletBase : BulletBase
    {
    #region 定義



    #endregion

    #region 変数

    #region SerializeField

    /// <summary>
    /// 弾の動きデータ
    /// </summary>
    [SerializeField]
    private BallisticStatusData ballisticStatusData;

    #endregion

    #region Public



    #endregion

    #region Private
        
        /// <summary>
        /// 弾動きイベント
        /// </summary>
        protected UnityAction bulletAction = null;

    #endregion

    #endregion

    #region Method

    #region Public

        /// <summary>
        /// 外部設定
        /// </summary>
        /// <param name="data">弾データ</param>
        public void Setting(BallisticStatusData data)
        {
            ballisticStatusData = data;
    
            // 弾の挙動決定
            SetAction();
        }

    #endregion

    #region Private

        /// <summary>
        /// 弾のアクション決定
        /// </summary>
        protected void SetAction()
        {
            switch (ballisticStatusData.ballisticType)
            {
                case BulletFunction.eBallisticType.Straight:
                    bulletAction = () =>
                    {
                        //Straight().Forget();
                        Straight_Update();
                    };
                    break;
        
                case BulletFunction.eBallisticType.Homing:
                    break;
            }
        }
    
        /// <summary>
        /// ストレート
        /// </summary>
        protected async UniTaskVoid Straight()
        {
            try
            {
                var cancelToken = this.GetCancellationTokenOnDestroy();
                
                while (true)
                {
                    if (ballisticStatusData != null && targetTransform != null)
                    {
                        targetTransform.Translate(0, ballisticStatusData.bulletSpeed, 0);
                    }
                
                    await UniTask.Yield(cancelToken);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        protected void Straight_Update()
        {
            if (ballisticStatusData != null && targetTransform != null)
            {
                targetTransform.Translate(0, ballisticStatusData.bulletSpeed, 0);
            }
        }

    #endregion

    #region UnityMethod

        protected virtual void OnDestroy()
        {
            
        }

    #endregion

    #endregion
    }
}