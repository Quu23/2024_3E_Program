﻿using System;

namespace ShootingGame
{
    /// <summary>
    /// 使用する画像全てのURIを管理するクラス。
    /// </summary>
    internal class ImageUris
    {
        public static readonly Uri PLAYER = new Uri("../../../img/Player.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET = new Uri("../../../img/Player_Bullet.png", UriKind.RelativeOrAbsolute);
    }
}
