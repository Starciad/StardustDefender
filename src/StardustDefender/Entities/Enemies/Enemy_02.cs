﻿using Microsoft.Xna.Framework;

using StardustDefender.Controllers;
using StardustDefender.Core.Components;
using StardustDefender.Core.Engine;
using StardustDefender.Core.Entities.Register;
using StardustDefender.Core.Entities.Templates;
using StardustDefender.Core.Enums;
using StardustDefender.Core.Managers;
using StardustDefender.Effects;
using StardustDefender.Enums;

using System.Threading.Tasks;

namespace StardustDefender.Entities.Enemies
{
    [SEntityRegister(typeof(Header))]
    internal sealed class Enemy_02 : SEnemyEntity
    {
        // ==================================================== //

        private sealed class Header : SEntityHeader
        {
            protected override void OnProcess()
            {
                this.Classification = SEntityClassification.Enemy;
            }

            protected override bool OnSpawningCondition()
            {
                return SDifficultyController.DifficultyRate >= 2;
            }
        }

        // ==================================================== //

        private const float SHOOT_SPEED = 2f;
        private const float SHOOT_LIFE_TIME = 25f;

        private readonly STimer movementTimer = new(20f);
        private readonly STimer shootTimer = new(35f);

        private Direction movementDirection;

        // ==================================================== //
        // RESET
        public override void Reset()
        {
            this.Animation.Reset();
            this.Animation.Clear();

            this.Animation.SetMode(SAnimationMode.Forward);
            this.Animation.SetTexture(STextures.GetTexture("ENEMIES_Aliens"));
            this.Animation.AddSprite(STextures.GetSprite(32, 0, 1));
            this.Animation.AddSprite(STextures.GetSprite(32, 1, 1));
            this.Animation.SetDuration(3f);

            this.Team = STeam.Bad;

            this.HealthValue = 3;
            this.DamageValue = 1;

            this.ChanceOfKnockback = 0;
            this.KnockbackForce = 0;
        }

        // OVERRIDE
        protected override void OnAwake()
        {
            Reset();
        }
        protected override void OnStart()
        {
            this.movementTimer.Restart();
            this.shootTimer.Restart();
        }
        protected override void OnUpdate()
        {
            TimersUpdate();

            // Behaviour
            CollideWithPlayer();

            // AI
            MovementUpdate();
            ShootUpdate();
        }
        protected override void OnDamaged(int value)
        {
            _ = SSounds.Play("Damage_02");
            _ = SEffectsManager.Create<ImpactEffect>(this.WorldPosition);

            _ = Task.Run(async () =>
            {
                this.Color = Color.Red;
                await Task.Delay(235);
                this.Color = Color.White;
            });
        }
        protected override void OnDestroy()
        {
            SLevelController.EnemyKilled();

            _ = SSounds.Play("Explosion_01");
            _ = SEffectsManager.Create<ExplosionEffect>(this.WorldPosition);

            // Drop
            if (SRandom.Chance(20, 100))
            {
                _ = SItemsManager.CreateRandomItem(this.WorldPosition);
            }
        }

        // UPDATE
        private void TimersUpdate()
        {
            this.movementTimer.Update();
            this.shootTimer.Update();
        }
        private void MovementUpdate()
        {
            if (this.movementTimer.IsFinished)
            {
                this.movementTimer.Restart();
                switch (this.movementDirection)
                {
                    case Direction.Horizontal:
                        int direction = SRandom.Chance(50, 100) ? -1 : 1;
                        this.LocalPosition = new(this.LocalPosition.X + direction, this.LocalPosition.Y);
                        this.movementDirection = Direction.Vertical;
                        break;

                    case Direction.Vertical:
                        this.LocalPosition = new(this.LocalPosition.X, this.LocalPosition.Y + 1);
                        this.movementDirection = Direction.Horizontal;
                        break;
                }
            }
        }
        private void ShootUpdate()
        {
            if (this.shootTimer.IsFinished)
            {
                this.shootTimer.Restart();
                Shoot();
            }
        }

        // SKILLS
        private void Shoot()
        {
            _ = SSounds.Play("Shoot_01");
            SProjectileManager.Create(new()
            {
                SpriteId = 0,
                Team = STeam.Bad,
                Position = new(this.WorldPosition.X, this.WorldPosition.Y + 16f),
                Speed = new(0, SHOOT_SPEED),
                Damage = this.DamageValue,
                LifeTime = SHOOT_LIFE_TIME,
                Range = 10f
            });
        }
    }
}
