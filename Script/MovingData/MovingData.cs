using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "MovingData", menuName = "STGGame/MovingData")]
public class MovingData : ScriptableObject
{
    /// <summary>
    /// ウェイポイント
    /// </summary>
    public List<Vector3> waypointList;
    
    /// <summary>
    /// 移動時間
    /// </summary>
    public float duration;

    /// <summary>
    /// 何秒後に移動を開始するか
    /// </summary>
    public float delay;
    
    /// <summary>
    /// イージングタイプ
    /// </summary>
    public Ease easeType;

    /// <summary>
    /// ループ回数
    /// </summary>
    public int loopNum;

}
