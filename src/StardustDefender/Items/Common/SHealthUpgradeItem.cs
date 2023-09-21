﻿using StardustDefender.Controllers;

using Microsoft.Xna.Framework;
using StardustDefender.Engine;
using StardustDefender.Enums;
using StardustDefender.Entities.Player;

namespace StardustDefender.Items.Common
{
    internal sealed class SHealthUpgradeItem : SItemTemplate
    {
        protected override void OnInitialize()
        {
            Animation.SetTexture(STextures.GetTexture("Upgrades"));
            Animation.AddSprite(STextures.GetSprite(SPRITE_SCALE, 0, 0));
        }

        protected override void OnEffect(SPlayerEntity player)
        {
            player.HealthValue += 1;
        }
    }
}
