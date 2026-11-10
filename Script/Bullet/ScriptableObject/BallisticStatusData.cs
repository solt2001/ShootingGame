using UnityEngine;
using STGGame.Bullet;

/// <summary>
/// 弾道ステータス
/// </summary>
[CreateAssetMenu(fileName = "BallisticStatusData", menuName = "STGGame/BallisticStatusData")]
public class BallisticStatusData : ScriptableObject
{
    /// <summary>
    /// 弾道タイプ
    /// </summary>
    public BulletFunction.eBallisticType ballisticType;
    
    /// <summary>
    /// 弾のスピード
    /// </summary>
    public float bulletSpeed;
}
