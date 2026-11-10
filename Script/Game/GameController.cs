using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

namespace STGGame.Controller
{
    public class GameController : GameControllerBase
    {
    #region 定義

        

    #endregion
        
    #region 変数

        #region SerializeField
        
        /// <summary>
        /// 敵Prefab
        /// </summary>
        [Header("GameControllerクラス")]
        [SerializeField]
        private GameObject enemyPrefab;

        /// <summary>
        /// 敵スポーンペアレント
        /// </summary>
        [SerializeField]
        private Transform enemySpawnParent;
        
        /// <summary>
        /// スポーンエリア
        /// </summary>
        [SerializeField]
        private BoxCollider2D area;
        
        /// <summary>
        /// スポーン間隔
        /// </summary>
        [SerializeField]
        private float spawnInterval;

        /// <summary>
        /// スポーン上限
        /// </summary>
        [SerializeField]
        private int spawnLimit = 0;
        
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
        /// エネミー自動スポーン
        /// </summary>
        private async UniTaskVoid EnemySpawnAsync()
        {
            while (true)
            {
                // スポーン上限までいったらスポーンを止める
                if (enemySpawnParent.childCount >= spawnLimit)
                {
                    await UniTask.Yield();
                    
                    continue;
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(spawnInterval));
                
                // コライダーの中心座標とサイズ（スケール反映済み）を取得
                Vector2 center = area.bounds.center;
                Vector2 size = area.bounds.size;

                // ランダムなオフセットを計算
                float randomX = Random.Range(-size.x / 2f, size.x / 2f);
                float randomY = Random.Range(-size.y / 2f, size.y / 2f);
                
                Vector2 spawnPos = new Vector2(center.x + randomX, center.y + randomY);
                
                await InstantiateAsync(enemyPrefab, enemySpawnParent, spawnPos, enemyPrefab.transform.rotation);

                await UniTask.Yield();
            }
        }

    #endregion

    #region UnityMethod

        protected override void Start()
        {
            //EnemySpawnAsync().Forget();
            base.Start();
        }

    #endregion

    #endregion
    }
}

