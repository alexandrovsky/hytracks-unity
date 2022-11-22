using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript.Behaviors.Cursors;
using System;
using TouchScript.Pointers;
using System.Security.Cryptography;

namespace HyTracks {
    public class HyTracksObjectCursorBase:ObjectCursor {

		public int objectId;
		[Header("Parameters")]
		public HyTracksSteepParameters parameters;
		[Header("Prefabs")]
		public GameObject parametersPefabUI;
		public GameObject connectionPrefab;


		[Header("Pie Menu UIs")]
		public RectTransform uiInputSocial;
		public RectTransform uiInputTechnology;
		public RectTransform uiInputEconomic;
		public RectTransform uiInputEnvirontment;
		public RectTransform uiInputPolitics;


		public HyTracksParametersList GetInputParameters(STEEPDimension steep)
		{
			return parameters.parametersInput[steep];
		}

		public HyTracksParametersList GetOutputParameters(STEEPDimension steep)
		{
			return parameters.parametersOutput[steep];
		}


		void InitUI() {
			foreach(STEEPDimension dim in Enum.GetValues(typeof(STEEPDimension))) {
				HyTracksParametersList paramList = GetInputParameters(dim);
				foreach(HyTracksParametersBase p in paramList.parameters) {
					if(p.isVisible) {
						RectTransform uiParent = null;
						switch(dim) {
						case STEEPDimension.SOCIAL:
							uiParent = uiInputSocial;
							break;
						case STEEPDimension.TECHNOLOGY:
							uiParent = uiInputTechnology;
							break;
						case STEEPDimension.ECONOMICS:
							uiParent = uiInputTechnology;
							break;
						case STEEPDimension.ENVIRONMENT:
							uiParent = uiInputEnvirontment;
							break;
						case STEEPDimension.POLITICS:
							uiParent = uiInputPolitics;
							break;
						}
						if(uiParent == null) {
							break;
						}

						GameObject newUI = Instantiate(parametersPefabUI,uiParent);
						HyTracksParametersUI ui = newUI.GetComponent<HyTracksParametersUI>();
						ui.Init(p);
						LayoutRebuilder.ForceRebuildLayoutImmediate(uiParent);
					}					
				}
			}
			
		}

		public new void Init(RectTransform parent, IPointer pointer)
		{
			base.Init(parent, pointer);
			parameters.LoadDataFromJSONFiles();
			InitUI();
		}

		//private void Start()
		//{
		//	InitUI();
		//}

	}

}


