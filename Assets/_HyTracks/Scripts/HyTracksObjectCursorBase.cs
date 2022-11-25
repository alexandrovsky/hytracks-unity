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
using UnityEngine.UI.Extensions.Examples.FancyScrollViewExample06;
using Unity.VisualScripting;

namespace HyTracks {
    public class HyTracksObjectCursorBase:ObjectCursor {

		public int objectId;
		[Header("Parameters")]
		public HyTracksSteepParameters parameters;
		[Header("Prefabs")]
		public GameObject parametersPefabUI;

		[Header("Theme Settings")]
		public HyTracksThemeSettings themeSettings;

		[Header("Pie SubMenu UIs")]
		public uPIeMenu pieMenu;
		public RectTransform uiSocial;
		public RectTransform uiTechnology;
		public RectTransform uiEconomic;
		public RectTransform uiEnvirontment;
		public RectTransform uiPolitics;

		[Header("Dynamic Panels Inputs")]
		[SerializeField]
		DynamicPanelsCanvas dpcSocInput;
		[SerializeField]
		DynamicPanelsCanvas dpcTecInput;
		[SerializeField]
		DynamicPanelsCanvas dpcEcoInput;
		[SerializeField]
		DynamicPanelsCanvas dpcEnvInput;
		[SerializeField]
		DynamicPanelsCanvas dpcPolInput;

		[Header("Dynamic Panels Outputs")]
		[SerializeField]
		DynamicPanelsCanvas dpcSocOutput;
		[SerializeField]
		DynamicPanelsCanvas dpcTecOutput;
		[SerializeField]
		DynamicPanelsCanvas dpcEcoOutput;
		[SerializeField]
		DynamicPanelsCanvas dpcEnvOutput;
		[SerializeField]
		DynamicPanelsCanvas dpcPolOutput;


		[EditorCools.Button]
		void UpdatePieMenu()
		{
			foreach (var pie in transform.GetComponentsInChildren<uPIeMenu>())
			{
				if (pie.name.Contains("Input"))
				{
					var parent = pie.transform.parent.GetComponentInParent<uPIeMenu>();
					pie.StartDegOffset = parent.StartDegOffset;
				}else if (pie.name.Contains("Output"))
				{
					var parent = pie.transform.parent.GetComponentInParent<uPIeMenu>();
					pie.StartDegOffset = parent.StartDegOffset + 32;
				}
				pie.Realign();
			}
			//pieMenu.Realign();
			

			//foreach (var btn in transform.GetComponentsInChildren<Selectable>())
			//{
			//	(btn.transform as RectTransform).rotation = Quaternion.identity;
			//}
		}


		public HyTracksParametersList GetInputParameters(STEEPDimension steep)
		{
			return parameters.parametersInput[steep];
		}

		public HyTracksParametersList GetOutputParameters(STEEPDimension steep)
		{
			return parameters.parametersOutput[steep];
		}

		void BuildParametersForDimension(STEEPDimension dimension, HyTracksParametersList parametersList, RectTransform uiParent, DynamicPanelsCanvas dpc, Sprite sprite) {
			Panel panel = null;
			for (int i = 0; i < parametersList.parameters.Count; i++)
			{
				GameObject newUI = Instantiate(parametersPefabUI, uiParent);
				newUI.GetComponent<Image>().enabled = true;
				newUI.GetComponent<Image>().color = themeSettings.ThemeForDimension(dimension).color;
				HyTracksParametersUI ui = newUI.GetComponent<HyTracksParametersUI>();
				ui.Init(parametersList.parameters[i], dpc);
				
				if (panel == null)
				{
					panel = PanelUtils.CreatePanelFor(newUI.transform as RectTransform, dpc);
					
					panel[0].Label = ui.parameters.name;
					panel[0].Icon = sprite;
					panel.DockToPanel(dpc.RootPanelGroup[0], Direction.Left);
					panel.FloatingSize = themeSettings.parametersPanelFloatingSize;
				}
				else
				{
					PanelTab tab = panel.AddTab(newUI.transform as RectTransform);
					tab.Label = ui.parameters.name;
					tab.Icon = sprite;
				}
			}
			dpc.ForceRebuildLayoutImmediate();
			
			panel.ResizeTo(new Vector2(256, 256));
			
		}

		

		private void PanelNotificationCenter_OnStoppedDraggingTab(PanelTab tab)
		{
			Debug.Log($"Tab Drag Stop {tab.name} {tab.Index} {tab.Panel.name} {tab.Panel.IsDocked}");
		}

		private void PanelNotificationCenter_OnStartedDraggingTab(PanelTab tab)
		{
			Debug.Log($"Tab Drag Start {tab.name} {tab.Index}");			
		}

		void GetComponentsForDimension(HyTracksParametersType type, STEEPDimension dim, out DynamicPanelsCanvas dpc, out RectTransform uiParent, out Sprite sprite)
		{
			dpc = null;
			uiParent = null;
			sprite = null;

			switch (dim)
			{
				case STEEPDimension.SOCIAL:
					uiParent = uiSocial;
					if (type == HyTracksParametersType.INPUT)
					{						
						dpc = dpcSocInput;
					}
					else
					{
						dpc = dpcSocOutput;
					}
					
					sprite = themeSettings.themeSoc.sprite;
					break;
				case STEEPDimension.TECHNOLOGY:
					uiParent = uiTechnology;
					if (type == HyTracksParametersType.INPUT)
					{
						dpc = dpcTecInput;
					}
					else
					{
						dpc = dpcTecOutput;
					}
					sprite = themeSettings.themeTec.sprite;
					break;
				case STEEPDimension.ECONOMICS:
					uiParent = uiEconomic;
					if (type == HyTracksParametersType.INPUT)
					{
						dpc = dpcEcoInput;
					}
					else
					{
						dpc = dpcEcoOutput;
					}
					
					sprite = themeSettings.themeEco.sprite;
					break;
				case STEEPDimension.ENVIRONMENT:
					uiParent = uiEnvirontment;
					if (type == HyTracksParametersType.INPUT)
					{
						dpc = dpcEnvInput;
					}
					else
					{
						dpc = dpcEnvOutput;
					}
					sprite = themeSettings.themeEnv.sprite;
					break;
				case STEEPDimension.POLITICS:
					uiParent = uiPolitics;
					if (type == HyTracksParametersType.INPUT)
					{
						dpc = dpcPolInput;
					}
					else
					{
						dpc = dpcPolOutput;
					}

					sprite = themeSettings.themePol.sprite;
					break;

			}
		}

		void InitUI() {

			PanelNotificationCenter.OnStartedDraggingTab += PanelNotificationCenter_OnStartedDraggingTab;
			PanelNotificationCenter.OnStoppedDraggingTab += PanelNotificationCenter_OnStoppedDraggingTab;

			foreach (STEEPDimension dim in Enum.GetValues(typeof(STEEPDimension))) {

				DynamicPanelsCanvas dpc = null;
				RectTransform uiParent = null;
				Sprite sprite = null;
				GetComponentsForDimension(HyTracksParametersType.INPUT, dim, out dpc, out uiParent, out sprite);
				HyTracksParametersList paramList = GetInputParameters(dim);
				BuildParametersForDimension(dim, paramList, uiParent, dpc, sprite);


				UpdatePieMenu();
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


