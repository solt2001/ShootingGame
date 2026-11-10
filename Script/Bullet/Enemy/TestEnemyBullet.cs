using STGGame.Bullet;
using UnityEngine;
using UnityEngine.Events;

public class TestEnemyBullet : EnemyBulletBase
{
#region 定義

    

#endregion
    
#region 変数

    #region SerializeField



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

    protected override void Start()
    {
        // if (bulletAction != null)
        // {
        //     bulletAction.Invoke();
        // }
    }

    protected override void Update()
    {
        if (bulletAction != null)
        {
            bulletAction.Invoke();
        }
    }

#endregion

#endregion
}