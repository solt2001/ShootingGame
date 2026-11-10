using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using STGGame.InputController;
using TMPro;

namespace STGGame.Controller
{
    public class GameControllerBase : SingletonMonoBehaviour<GameControllerBase>
    {
    #region 定義

        

    #endregion
        
    #region 変数

        #region SerializeField

        /// <summary>
        /// 使用するプレイヤープレハブ
        /// </summary>
        [Header("基底クラス参照")]
        [SerializeField]
        protected GameObject playerPrefab;

        /// <summary>
        /// スタートポジション
        /// </summary>
        [SerializeField]
        protected Transform startPosition;

        /// <summary>
        /// プレイヤースポーン先親オブジェクト
        /// </summary>
        [SerializeField]
        protected Transform playerSpawnParent;

        public Transform PlayerSpawnParent
        {
            get { return playerSpawnParent; } 
        }
        
        /// <summary>
        /// 敵の弾をスポーンさせるための親トランスフォーム
        /// </summary>
        [SerializeField]
        protected Transform enemyBulletSpawnParent;
        public Transform EnemyBulletSpawnParent
        {
            get { return enemyBulletSpawnParent; }
        }
        
        /// <summary>
        /// プレイヤー弾スポーン先親トランスフォーム
        /// </summary>
        [SerializeField]
        protected Transform playerBulletSpawnParent;

        public Transform PlayerBulletSpawnParentTransform
        {
            get { return playerBulletSpawnParent; }
        }
        
        /// <summary>
        /// アイテムスポーン先
        /// </summary>
        [SerializeField]
        protected Transform ItemSpawnParent;

        public Transform ItemSpawnParentTransform
        {
            get { return ItemSpawnParent; }
        }
        
        /// <summary>
        /// インゲーム用BGマテリアル
        /// </summary>
        [SerializeField]
        protected Material inGameBGMaterial;
        
        /// <summary>
        /// スクロール速度
        /// </summary>
        [SerializeField]
        protected float bgScrollSpeed = 0.05f;

        /// <summary>
        /// 行動制限最大値
        /// </summary>
        [SerializeField]
        protected Transform moveClampMaxPoint;

        /// <summary>
        /// 行動制限最小値
        /// </summary>
        [SerializeField]
        protected Transform moveClampMinPoint;
        
        /// <summary>
        /// パワーテキスト
        /// </summary>
        [SerializeField]
        protected TMP_Text powerNumText;
        
        #endregion

        #region Public

        /// <summary>
        /// 行動制限最大値
        /// </summary>
        public Vector2 MoveClampMax
        {
            get { return new Vector2(moveClampMaxPoint.position.x, moveClampMaxPoint.position.y); }
        }

        /// <summary>
        /// 行動制限最小値
        /// </summary>
        public Vector2 MoveClampMin
        {
            get { return new Vector2(moveClampMinPoint.position.x, moveClampMinPoint.position.y); }
        }

        #endregion

        #region Private

        protected override bool dontDestroyOnLoad { get; }

        /// <summary>
        /// プレイヤーのオブジェクトインスタンス
        /// </summary>
        protected GameObject playerInstance;

        public GameObject PlayerPrefab
        {
            get
            {
                if (playerInstance != null)
                {
                    return playerInstance;
                }
                
                return null;
            }
        }

        /// <summary>
        /// プレイヤーのコントロールクラス
        /// </summary>
        protected PlayerControllerBase playerController;
        
        /// <summary>
        /// 背景スクロールタスク用トークン
        /// </summary>
        protected CancellationTokenSource bgScrollToken;
        
        #endregion
        
    #endregion
        
    #region Method

    #region Public


        
    #endregion

    #region Private

        /// <summary>
        /// プレイヤースポーン
        /// </summary>
        protected virtual void PlayerSpawn()
        {
            // プレイヤーをスポーン
            playerInstance =  Instantiate(playerPrefab, startPosition.position, startPosition.rotation, playerSpawnParent);
        
            // プレイヤーコントローラーを取得
            playerController = playerInstance.GetComponent<PlayerControllerBase>();
            
            // コントロールを有効化
            playerController.ControllStart();
        }

        /// <summary>
        /// 背景スクロールタスク
        /// </summary>
        protected virtual async UniTaskVoid BGScroll()
        {
            try
            {
                bgScrollToken = new CancellationTokenSource();

                float y = 0.0f;
                
                string texName = "_MainTex";
                
                while (true)
                {
                    if (y >= 10.0f)
                    {
                        y = 0.0f;
                    }
                    
                    inGameBGMaterial.SetTextureOffset(texName, new Vector2(0.0f, y));

                    y += bgScrollSpeed;
                    
                    await UniTask.Yield(bgScrollToken.Token);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }
        
    #endregion
        
    #region UnityMethod 
            
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected virtual void Start() 
        {
            PlayerSpawn();
            
            BGScroll().Forget();
        }

        // Update is called once per frame
        protected void Update()
        {
        
        }

    #endregion

    #endregion
    }
}