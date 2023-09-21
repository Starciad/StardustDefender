﻿using Microsoft.Xna.Framework;

namespace StardustDefender.Engine
{
    internal static class STime
    {
        internal static GameTime UpdateTime { get; private set; }
        internal static GameTime DrawTime { get; private set; }

        internal static void Update(GameTime updateTime = null, GameTime drawTime = null)
        {
            if (updateTime != null) UpdateTime = updateTime;
            if (drawTime != null) DrawTime = drawTime;
        }
    }
}
