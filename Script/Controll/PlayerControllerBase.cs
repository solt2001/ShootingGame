using System;
using System.Threading;
using STGGame.Player;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
using STGGame.Controller;

namespace STGGame.InputController
{
    /// <summary>
    /// ユーザーの入力から機体を制御する
    /// </summary>
    public class PlayerControllerBase : MonoBehaviour
    {
    #region 定義



    #endregion

    #region 変数

        #region SerializeField
            
        /// <summary>
        /// 機体のトランスフォーム
        /// </summary>
        [SerializeField]
        protected Transform targetTransform;

        /// <summary>
        /// プレイヤーの性能制御コンポーネント
        /// </summary>
        [SerializeField]
        protected PlayerBase playerBase;

        [SerializeField]
        protected InputActionReference moveAction;

        /// <summary>
        /// ショットボタン
        /// </summary>
        [SerializeField]
        protected InputActionReference shotButtonAction;

        /// <summary>
        /// スロー移動ボタン
        /// </summary>
        [SerializeField]
        protected InputActionReference slowButtonAction;
        
        #endregion

        #region Public



        #endregion

        #region Private

        /// <summary>
        /// 入力受付コンポーネント
        /// </summary>
        protected InputSystem_Controll input;

        /// <summary>
        /// 入力マップ
        /// </summary>
        protected InputSystem_Controll.PlayerMapActions playerActions;

        /// <summary>
        /// 移動速度
        /// </summary>
        protected float moveSpeed;
        
        /// <summary>
        /// スロー移動倍率
        /// </summary>
        protected float slowMag;
        
        /// <summary>
        /// 弾を打つ間隔
        /// </summary>
        protected float bulletInterval;

        /// <summary>
        /// 弾発射タスク監視トークン
        /// </summary>
        protected CancellationTokenSource token;
        
        /// <summary>
        /// 移動制限を行うか
        /// </summary>
        protected bool isMoveClamp = false;

        /// <summary>
        /// スロー移動するか
        /// </summary>
        protected bool isSlowMove = false;
        
        #endregion

    #endregion


    #region Method

        #region Public

        /// <summary>
        /// 外部設定用
        /// </summary>
        /// <param name="moveSpeed">移動速度</param>
        /// <param name="bulletInterval"></param>
        public void Setting(float moveSpeed, float slowMag, float bulletInterval)
        {
            // 移動速度決定
            this.moveSpeed = moveSpeed;
            
            // スロー移動倍率決定
            this.slowMag = slowMag;
            
            // 打つ間隔決定
            this.bulletInterval = bulletInterval;
        }

        /// <summary>
        /// 移動の許可
        /// </summary>
        public void ControllStart()
        {
            if (input == null)
            {
                input = new InputSystem_Controll();
            }
            
            if (input != null)
            {
                input.Enable();
            }
        }

        /// <summary>
        /// 移動ストップ
        /// </summary>
        public void ControllStop()
        {
            input.Disable();
        }

        /// <summary>
        /// 入力情報の破棄
        /// </summary>
        public void ControllDispose()
        {
            input.Dispose();
        }
        
        /// <summary>
        /// ショットボタン押下時
        /// </summary>
        /// <param name="context"></param>
        public void OnShot(InputAction.CallbackContext context)
        {
            // 連打対策
            CancelBullet();
            
            token = new  CancellationTokenSource();
            
            Bullet(token.Token).Forget();
        }
        
        /// <summary>
        /// 射撃キャンセルイベント
        /// </summary>
        /// <param name="context"></param>
        public void ReleaseShot(InputAction.CallbackContext context)
        {
            CancelBullet();
        }

        public void OnBumb()
        {
            Debug.Log("ボム！");
        }

        /// <summary>
        /// 行動制限を行うか
        /// </summary>
        /// <param name="isMoveClamp"></param>
        public void SetIsMoveClamp(bool isMoveClamp)
        {
            if (this.isMoveClamp != isMoveClamp)
            {
                this.isMoveClamp = isMoveClamp;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 移動
        /// </summary>
        protected virtual void Move()
        {
            // キー入力から読み取り
            var vec2 = moveAction.action.ReadValue<Vector2>();

            targetTransform.Translate(vec2.x * moveSpeed, vec2.y * moveSpeed, targetTransform.position.z);

            // 移動制限
            if (isMoveClamp)
            {
                MoveClamp();
            }
        }
        
        /// <summary>
        /// 移動制限
        /// ※処理負荷になるが、Translateで移動しているとワープっぽい挙動になりコリジョンを貫通しようとする動きが見られるため、ここで補正
        /// </summary>
        protected virtual void MoveClamp()
        {
            var pos = transform.position;
            
            // 移動最大
            var boundMax = playerBase.PlayerSpriteBounds.extents;
            
            // 移動最小（-1で反転）
            var boundMin = playerBase.PlayerSpriteBounds.extents * -1;
            
            // Clampして最大/最小で丸める
            pos.x = Mathf.Clamp(pos.x, GameController.Instance.MoveClampMin.x - boundMin.x, GameController.Instance.MoveClampMax.x - boundMax.x);
            pos.y = Mathf.Clamp(pos.y, GameController.Instance.MoveClampMin.y - boundMin.y, GameController.Instance.MoveClampMax.y - boundMax.y);
            
            transform.position = pos;
        }
        
        /// <summary>
        /// 弾発射
        /// </summary>
        protected virtual async UniTaskVoid Bullet(CancellationToken token)
        {
            try
            {
                // 開始前に中断通知があれば例外を投げる
                token.ThrowIfCancellationRequested();
                
                while (true)
                {
                    // 一瞬待つ
                    await UniTask.Delay(TimeSpan.FromSeconds(bulletInterval), cancellationToken: token);
                
                    // 弾生成
                    await InstantiateAsync(playerBase.BulletPrefab, GameController.Instance.PlayerBulletSpawnParentTransform, targetTransform.position, Quaternion.identity);
                }
            }
            catch (OperationCanceledException)
            {
                // 正常に止まった時用のハンドリング
            }
        }
        
        /// <summary>
        /// 発射キャンセル
        /// </summary>
        protected virtual void CancelBullet()
        {
            if (token != null)
            {
                token.Cancel();
                token.Dispose();
                token = null;
            }
        }

        /// <summary>
        /// スロー移動有効化
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SlowMoveEnable(InputAction.CallbackContext context)
        {
            moveSpeed /= slowMag;
        }

        /// <summary>
        /// スロー移動無効化
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SlowMoveDisable(InputAction.CallbackContext context)
        {
            moveSpeed *= slowMag;
        }

        #endregion

        #region UnityMethod

        protected void Awake()
        {
            // ショットボタンイベント登録
            if (shotButtonAction != null)
            {
                shotButtonAction.action.started += OnShot;
                shotButtonAction.action.canceled += ReleaseShot;
                shotButtonAction.action.Enable();
            }

            // スロー移動有効化
            if (slowButtonAction != null)
            {
                slowButtonAction.action.started += SlowMoveEnable;
                slowButtonAction.action.canceled += SlowMoveDisable;
                slowButtonAction.action.Enable();
            }
        }

        protected void OnDisable()
        {
            if (shotButtonAction != null)
            {
                shotButtonAction.action.started -= OnShot;
                shotButtonAction.action.canceled -= ReleaseShot;
                shotButtonAction.action.Disable();    
            }
            
            if (slowButtonAction != null)
            {
                slowButtonAction.action.started -= SlowMoveEnable;
                slowButtonAction.action.canceled -= SlowMoveDisable;
                slowButtonAction.action.Disable();
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            
        }

        // Update is called once per frame
        protected void Update()
        {
            Move();
        }

        #endregion

    #endregion
    }
}
