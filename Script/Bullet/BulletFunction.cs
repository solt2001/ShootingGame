
namespace STGGame.Bullet
{
    /// <summary>
    /// 弾専用処理クラス
    /// </summary>
    public static class BulletFunction
    {
        /// <summary>
        /// 弾道タイプ
        /// </summary>
        public enum eBallisticType
        {
            /// <summary>
            /// 直線打ち
            /// </summary>
            Straight = 0,
        
            /// <summary>
            /// ホーミング
            /// </summary>
            Homing,
        }

        /// <summary>
        /// 射出タイプ
        /// </summary>
        public enum eFireType
        {
            /// <summary>
            /// 単発
            /// </summary>
            Once = 0,
        
            /// <summary>
            /// 扇状に射出する
            /// </summary>
            Ougi,
        
            /// <summary>
            /// 自機狙い単発
            /// </summary>
            AimOnce,
        }
    }
}
