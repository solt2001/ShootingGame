using UnityEngine;
using STGGame.Bullet;
using UnityEngine.Serialization;

/// <summary>
/// 射撃ステータス
/// </summary>
[CreateAssetMenu(fileName = "BulletStatusData", menuName = "STGGame/BulletStatusData")]
public class ShotStatusData : ScriptableObject
{
    /// <summary>
    /// 射出タイプ
    /// </summary>
    public BulletFunction.eFireType fireType;

    /// <summary>
    /// 使用するプレハブ
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// 弾の方向
    /// </summary>
    public float angle;

    /// <summary>
    /// 次の弾を発射するまでの間隔
    /// </summary>
    public float interval;

    /// <summary>
    /// 弾の数
    /// </summary>
    public int count;
    
    /// <summary>
    /// リピートする場合、次の弾を発射するまでの間隔
    /// </summary>
    public float repeatInterval;
    
    /// <summary>
    /// リピート回数
    /// </summary>
    public int repeatCount;
}
