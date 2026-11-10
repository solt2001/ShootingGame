using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using STGGame.Bullet;
using STGGame.Bullet.Player;
using STGGame.Controller;
using STGGame.Score;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace STGGame.Enemy
{
    /// <summary>
    /// 敵基底クラス
    /// </summary>
    public class EnemyBase : MonoBehaviour
    {
    #region 定義

        /// <summary>
        /// 弾幕のコマンド
        /// </summary>
        [Serializable]
        public class DanmakuCommand
        {
            /// <summary>
            /// 弾道ステータスデータ
            /// </summary>
            public BallisticStatusData ballisticStatusData;
            
            /// <summary>
            /// 射出ステータスデータ
            /// </summary>
            public ShotStatusData shotStatusData;
            
            /// <summary>
            /// 次のコマンド発火までの時間
            /// </summary>
            public float nextCommandInterval;
        }

        /// <summary>
        /// 移動のコマンド
        /// </summary>
        [Serializable]
        public class MovingCommand
        {
            /// <summary>
            /// ムーブコマンド
            /// </summary>
            public MovingData movingData;
            
            /// <summary>
            /// 次のコマンド発火までの時間
            /// </summary>
            public float nextCommandInterval;
        }

        /// <summary>
        /// 抽選アイテム
        /// </summary>
        [Serializable]
        public class LotteryItem
        {
            /// <summary>
            /// スポーンするアイテム
            /// </summary>
            public GameObject itemObj;
            
            /// <summary>
            /// 登録したアイテムの抽選確率
            /// </summary>
            public float lotteryChance;
            
            /// <summary>
            /// 抽選優先度
            /// </summary>
            public int priority;
        }

    #endregion
    
    #region 変数

    #region SerializeField
        /// <summary>
        /// 体力
        /// </summary>
        [SerializeField]
        protected float hp;

        /// <summary>
        /// 倒した時のスコア
        /// </summary>
        [SerializeField]
        protected long conbatScore;
        
        /// <summary>
        /// 弾幕設定
        /// </summary>
        [SerializeField]
        protected List<DanmakuCommand> danmakuCommands;

        /// <summary>
        /// コマンドをループするか
        /// </summary>
        [SerializeField]
        protected bool isDanmakuCommandLoop;
        
        /// <summary>
        /// 移動コマンド
        /// </summary>
        [SerializeField]
        protected List<MovingCommand> movingCommands;
        
        /// <summary>
        /// 移動コマンドをループするか
        /// </summary>
        [SerializeField]
        protected bool isMovingCommandLoop;
        
        /// <summary>
        /// スポーンするかもしれないアイテムのリスト
        /// </summary>
        [SerializeField]
        protected List<LotteryItem> lotteryItemList;

    #endregion

    #region Public

        

    #endregion

    #region Private
        
        /// <summary>
        /// 弾幕タスク用トークン
        /// </summary>
        CancellationTokenSource danmakuTokenSource;

        /// <summary>
        /// 移動タスク用トークン
        /// </summary>
        CancellationTokenSource movingTokenSource;
        
    #endregion
    
    #endregion
    
    #region Method

    #region Public

    

    #endregion

    #region Private

        /// <summary>
        /// 生成関数ラップ
        /// </summary>
        /// <param name="spawnObj">生成対象</param>
        /// <returns></returns>
        protected GameObject Instantiate(GameObject spawnObj)
        {
            return Instantiate(spawnObj, transform.position, transform.rotation);
        }

        /// <summary>
        /// 生成関数ラップ
        /// </summary>
        /// <param name="spawnObj">生成対象</param>
        /// <param name="rotation">発射角度</param>
        /// <returns></returns>
        protected GameObject Instantiate(GameObject spawnObj, Quaternion rotation)
        {
            return Instantiate(spawnObj, transform.position, rotation);
        }
        
        /// <summary>
        /// 生成関数ラップ
        /// </summary>
        /// <param name="spawnObj">生成対象</param>
        /// <param name="position">発射位置</param>
        /// <returns></returns>
        protected GameObject Instantiate(GameObject spawnObj, Vector3 position)
        {
            return Instantiate(spawnObj, position, transform.rotation);
        }
        
        /// <summary>
        /// 生成関数ラップ
        /// </summary>
        /// <param name="spawnObj">生成対象</param>
        /// <param name="position">発射位置</param>
        /// <param name="rotation">発射角度</param>
        /// <returns></returns>
        protected GameObject Instantiate(GameObject spawnObj, Vector3 position, Quaternion rotation)
        {
            return Instantiate(spawnObj, position, rotation, GameController.Instance.EnemyBulletSpawnParent);
        }
        
        /// <summary>
        /// 当たった時用処理
        /// </summary>
        protected virtual void Hit(float damage)
        {
            // ダメージ計算
            hp -= damage;
            
            // HPが0になったか判定
            if (hp <= 0)
            {
                ScoreFunction.AddScore(conbatScore);
                SpawnItemLottery();
                Destroy(this.gameObject);
            }
        }
    
        /// <summary>
        /// 移動タスク実行
        /// </summary>
        protected virtual void EnableMove()
        {
            movingTokenSource = new CancellationTokenSource();
            
            MovingCommandProc().Forget();
        }

        /// <summary>
        /// 移動タスク終了
        /// </summary>
        protected virtual void DisableMove()
        {
            if (movingTokenSource != null)
            {
                movingTokenSource.Cancel();
                movingTokenSource.Dispose();
                movingTokenSource = null;
            }
        }

        /// <summary>
        /// 移動タスク
        /// </summary>
        protected virtual async UniTaskVoid MovingCommandProc()
        {
            try
            {
                var token = movingTokenSource.Token;
                
                if (movingCommands == null || movingCommands.Count <= 0)
                {
                    DisableMove();
                    await UniTask.Yield(token);
                }
                
                // 実行コマンドインデックス
                int commandIndex = 0;
                
                while (true)
                {
                    // 登録されているコマンド以上になったらやめる
                    if (commandIndex > movingCommands.Count - 1)
                    {
                        if (isMovingCommandLoop)
                        {
                            commandIndex = 0;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    // コマンド取得
                    var command = movingCommands[commandIndex];

                    // 移動実行
                    await Moving(command.movingData, token);
                    
                    // 次の移動まで待つ
                    if (command.nextCommandInterval > 0.0f)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(command.nextCommandInterval), cancellationToken: token);
                    }
                    
                    commandIndex++;
                }
                
                // 移動完了したので用済み
                Destroy(this.gameObject);
                
                await UniTask.Yield(token);
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        /// <summary>
        /// 移動データを元に移動
        /// </summary>
        /// <param name="movingData"></param>
        /// <param name="token"></param>
        protected virtual async UniTask Moving(MovingData movingData, CancellationToken token)
        {
            try
            {
                // 移動完了フラグ
                bool isComplete = false;

                // 移動時間分待つ
                if (movingData.delay > 0.0f)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(movingData.delay), cancellationToken: token);
                }

                // パスに沿って移動
                transform.DOPath(movingData.waypointList.ToArray(), movingData.duration, pathMode: PathMode.TopDown2D)
                         .SetOptions(closePath:false)
                         .SetEase(movingData.easeType)
                         .onComplete = () => isComplete = true;

                // 移動完了まで待つ
                while (!isComplete)
                {
                    await UniTask.Yield(token);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }
        
        /// <summary>
        /// 弾幕コマンド実行タスク開始
        /// </summary>
        /// <param name="onFinish">終了処理</param>
        protected virtual void EnableDanmaku(UnityAction onFinish = null)
        {
            danmakuTokenSource = new CancellationTokenSource();
            
            DanmakuCommandProc(onFinish).Forget();
        }

        /// <summary>
        /// 弾幕コマンド実行タスク無効化
        /// </summary>
        protected virtual void DisableDanmaku()
        {
            if (danmakuTokenSource != null)
            {
                danmakuTokenSource.Cancel();
                danmakuTokenSource.Dispose();
                danmakuTokenSource = null;
            }
        }

        /// <summary>
        /// 弾幕コマンド実行タスク
        /// </summary>
        /// <param name="onFinish">終了処理</param>
        protected virtual async UniTaskVoid DanmakuCommandProc(UnityAction onFinish = null)
        {
            try
            {
                int commandIndex = 0;

                var token = danmakuTokenSource.Token;
                
                while (true)
                {
                    // カウント以上になったら、ループしていないなら終わる
                    if (commandIndex > danmakuCommands.Count - 1)
                    {
                        // ループするならコマンドを最初からやり直す
                        if (isDanmakuCommandLoop)
                        {
                            commandIndex = 0;
                        }
                        else
                        {
                            break;   
                        }
                    }
                    
                    // コマンド取り出し
                    var command = danmakuCommands[commandIndex];
                
                    // 弾生成
                    await ShootingProc(command.shotStatusData, command.ballisticStatusData, token);
                    
                    // 一瞬待つ
                    await UniTask.Delay(TimeSpan.FromSeconds(command.nextCommandInterval), cancellationToken: token);
                    
                    commandIndex++;
                }
                
                // 終了処理
                onFinish?.Invoke();
            }
            catch (OperationCanceledException)
            {
                // 終了処理
                onFinish?.Invoke();
            }
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        /// <param name="shotStatusData">射出ステータスデータ</param>
        /// <param name="ballisticStatusData">弾道ステータスデータ</param>
        /// <param name="token"></param>
        protected virtual async UniTask ShootingProc(ShotStatusData shotStatusData, BallisticStatusData ballisticStatusData, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            // リピート回数
            int repeatCount = 0;
            
            // リピート回数分ループ
            while (repeatCount < shotStatusData.repeatCount + 1)
            {
                // 弾を撃つ
                await ShotPatternDecision(shotStatusData, ballisticStatusData, token);
                
                // 次のリピートまで待つ
                await UniTask.Delay(TimeSpan.FromSeconds(shotStatusData.repeatInterval), cancellationToken: token);
                
                repeatCount++;
            }
        }

        /// <summary>
        /// 弾を撃つ
        /// </summary>
        /// <param name="shotStatusData"></param>
        /// <param name="ballisticStatusData"></param>
        /// <param name="token"></param>
        protected async UniTask ShotPatternDecision(ShotStatusData shotStatusData, BallisticStatusData ballisticStatusData, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            switch (shotStatusData.fireType)
            {
                // 単発撃ち
                case BulletFunction.eFireType.Once:
                    await OnceShot(shotStatusData, ballisticStatusData, token);
                    break;
                
                // 扇型に広がる
                case BulletFunction.eFireType.Ougi:
                    await OugiShot(shotStatusData, ballisticStatusData, token);
                    break;
                
                // 射出時に自機方向へ飛ばす
                case BulletFunction.eFireType.AimOnce:
                    await AimOnceShot(shotStatusData, ballisticStatusData, token);
                    break;
                
                default:
                    return;
            }
        }

        /// <summary>
        /// 単発打ち
        /// </summary>
        /// <param name="shotStatusData"></param>
        /// <param name="ballisticStatusData"></param>
        /// <param name="token"></param>
        protected async UniTask OnceShot(ShotStatusData shotStatusData, BallisticStatusData ballisticStatusData, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            // 弾の数
            int count = 0;
                
            // 弾の数分ループ
            while (count < shotStatusData.count)
            {
                // 弾生成
                var enemyBullet = Instantiate(shotStatusData.bulletPrefab).GetComponent<EnemyBulletBase>();
                enemyBullet.Setting(ballisticStatusData);
            
                await UniTask.Yield(token);
                    
                // 次の弾を出すまで待つ
                await UniTask.Delay(TimeSpan.FromSeconds(shotStatusData.interval),  cancellationToken: token);
                    
                count++;
            }
        }

        /// <summary>
        /// 扇型に広がる
        /// </summary>
        /// <param name="shotStatusData"></param>
        /// <param name="ballisticStatusData"></param>
        /// <param name="token"></param>
        protected async UniTask OugiShot(ShotStatusData shotStatusData, BallisticStatusData ballisticStatusData, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            int count = 0;
            
            while (count < shotStatusData.count)
            {
                // 発射角度計算
                Quaternion rotate = new Quaternion(transform.rotation.x,
                                                   transform.rotation.y,
                                                   transform.rotation.z + (count * shotStatusData.angle),
                                                   transform.rotation.w);
                
                // 弾生成
                var enemyBullet = Instantiate(shotStatusData.bulletPrefab, rotate).GetComponent<EnemyBulletBase>();
                enemyBullet.Setting(ballisticStatusData);
            
                await UniTask.Yield(token);
                    
                count++;
            }
        }

        /// <summary>
        /// 射出時に自機方向へ飛ばす
        /// </summary>
        /// <param name="shotStatusData"></param>
        /// <param name="ballisticStatusData"></param>
        /// <param name="token"></param>
        protected async UniTask AimOnceShot(ShotStatusData shotStatusData, BallisticStatusData ballisticStatusData, CancellationToken token)
        {
            // キャンセルが入っていたら例外
            token.ThrowIfCancellationRequested();
            
            int count = 0;
            
            // プレイヤーのトランスフォーム取得　
            var playerTransform = GameController.Instance.PlayerPrefab.transform;
            
            // 発射分だけループ
            while (count < shotStatusData.count)
            {
                token.ThrowIfCancellationRequested();
                
                var playerPosition = playerTransform.position;
                var position = transform.position;
                
                // 向きたい方向を計算
                Vector3 dir =  playerPosition - position;
                
                // 発射角度計算
                Quaternion rotate = Quaternion.FromToRotation(Vector3.up, dir);
                
                // 弾生成
                var enemyBullet = Instantiate(shotStatusData.bulletPrefab, rotate).GetComponent<EnemyBulletBase>();
                enemyBullet.Setting(ballisticStatusData);
                
                // 次の弾を出すまで待つ
                await UniTask.Delay(TimeSpan.FromSeconds(shotStatusData.interval), cancellationToken: token);
                
                count++;
            }
        }

        /// <summary>
        /// アイテム抽選
        /// </summary>
        protected void SpawnItemLottery()
        {
            // スポーンするアイテムのインデックスを抽選
            int spawnItemIndex = ItemFunction.LotteryItem(lotteryItemList);
            
            if (spawnItemIndex != -1)
            {
                var spawnItemObj = Instantiate(lotteryItemList[spawnItemIndex].itemObj);
            }
        }

    #endregion
    
    #region UnityMethod

        protected virtual void Awake()
        {
            
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start()
        {
            EnableMove();
        }

        protected virtual void OnEnable()
        {
            
        }
        
        protected virtual void OnDestroy()
        {
            DisableDanmaku();
            DisableMove();
        }

        /// <summary>
        /// 当たり判定
        /// </summary>
        /// <param name="collider"></param>
        protected void OnCollisionEnter2D(Collision2D collider)
        {
            // プレイヤー弾に当たったら、当たった時の処理を走らせる
            if (collider.gameObject.CompareTag(TagDefine.PLAYER_BULLET))
            {
                var bullet = collider.gameObject.GetComponent<PlayerBulletBase>();
                Hit(bullet.BulletAttack);
            }
        }

    #endregion

    #endregion
    }
}
