using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript.Behaviors.Cursors;
using System;
using TouchScript.Pointers;
using System.Security.Cryptography;
using DynamicPanels;
using uPIe;

namespace HyTracks {
    public class HyTracksObjectCursorBase:ObjectCursor {

		public int objectId;
		[Header("Parameters")]
		public HyTracksSteepParameters parameters;
		[Header("Prefabs")]
		public GameObject parametersPefabUI;


		[Header("Pie Menu UIs")]
		public uPIeMenu pieMenu;
		public RectTransform uiInputSocial;
		public RectTransform uiInputTechnology;
		public RectTransform uiInputEconomic;
		public RectTransform uiInputEnvirontment;
		public RectTransform uiInputPolitics;

		[EditorCools.Button]
		void UpdatePieMenu()
		{
			foreach (var pie in transform.GetComponentsInChildren<uPIeMenu>())
			{
				pie.Realign();
			}
			pieMenu.Realign();
			foreach (var btn in transform.GetComponentsInChildren<Selectable>()) {
				(btn.transform as RectTransform).rotation = Quaternion.identity;
			}


		}


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
				foreach(HyTracksParametersBase parameters in paramList.parameters) {
					if(parameters.isVisible) {
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

						
						DynamicPanelsCanvas dpc = uiParent.GetComponent<DynamicPanelsCanvas>();
						

						GameObject newUI = Instantiate(parametersPefabUI, uiParent);
						
						HyTracksParametersUI ui = newUI.GetComponent<HyTracksParametersUI>();
						ui.Init(parameters);


						Panel panel = PanelUtils.CreatePanelFor(newUI.transform as RectTransform, dpc);
						panel[0].Label = ui.parameters.name;
						panel.DockToRoot(Direction.Top);						
						panel.FloatingSize = new Vector2(400, 400);
						//dpc.ForceRebuildLayoutImmediate();

						//(dpc.RootPanelGroup[0] as Panel).AddTab(panel.transform as RectTransform);
						//dpc.ForceRebuildLayoutImmediate();

						//if (dpc.RootPanelGroup.Count == 1)
						//{

						//}
						//else
						//{
						//	(dpc.RootPanelGroup[0] as Panel).AddTab(newUI.transform as RectTransform);
						//}

						//dpc.RootPanelGroup.AddElement(panel);
						//dpc.RootPanelGroup.DockToPanel(panel, Direction.Right);

						//RectTransform newUIrect = newUI.transform as RectTransform;
						//panel.AddTab(newUI);

						//PanelUtils.CreatePanelFor(newUI.transform as RectTransform, dpc);
						//LayoutRebuilder.ForceRebuildLayoutImmediate(uiParent);

						//dpc.ForceRebuildLayoutImmediate();
					}					
				}
			}
			
		}

		public new void Init(RectTransform parent, IPointer pointer)
		{
			base.Init(parent, pointer);
			parameters.LoadDataFromJSONFiles();
			//InitUI();
		}

		private void Start()
		{
			parameters.LoadDataFromJSONFiles();
			InitUI();
		}

	}

}


