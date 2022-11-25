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

		public Vector2 parametersUIBaseSize = new Vector2(256, 256);
		public Vector2 parametersPanelFloatingSize = new Vector2(400, 400);

		public float inrementStep = 1000;

		public SteepDimensionTheme ThemeForDimension(STEEPDimension dimension)
		{
			switch (dimension)
			{
				case STEEPDimension.SOCIAL:
					return themeSoc;
				case STEEPDimension.TECHNOLOGY:
					return themeTec;
				case STEEPDimension.ECONOMICS:
					return themeEco;
				case STEEPDimension.ENVIRONMENT:
					return themeEnv;				
				case STEEPDimension.POLITICS:
					return themePol;
				default:
					return null;
			}
		}
	}
}
