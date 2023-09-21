﻿using StardustDefender.Background;

using System;
using System.Collections.Generic;
using System.Linq;

namespace StardustDefender.Controllers
{
    internal static class SBackgroundController
    {
        internal static float GlobalParallaxFactor { get; set; }

        private static readonly List<SBackground> backgrounds = new();

        internal static void BeginRun()
        {
            GlobalParallaxFactor = 1f;

            foreach (Type type in SGame.Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(SBackground))))
            {
                SBackground background = (SBackground)Activator.CreateInstance(type);
                background.Initialize();

                backgrounds.Add(background);
            }
        }

        internal static void Update()
        {
            backgrounds.ForEach(x => x.Update());
        }

        internal static void Draw()
        {
            backgrounds.ForEach(x => x.Draw());
        }
    }
}
