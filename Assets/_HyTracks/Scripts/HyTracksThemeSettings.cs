using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HyTracks
{
	[CreateAssetMenu(menuName = "HyTracks/ThemeSettings")]
	public class HyTracksThemeSettings : ScriptableObject
    {
        [Serializable]
        public class SteepDimensionTheme {
            public Color color;
            public Sprite sprite;
        }

        public SteepDimensionTheme themeSoc;
		public SteepDimensionTheme themeTec;
		public SteepDimensionTheme themeEco;
		public SteepDimensionTheme themeEnv;
		public SteepDimensionTheme themePol;


		public Vector2 parametersPanelFloatingSize = new Vector2(400, 400);
	}
}
