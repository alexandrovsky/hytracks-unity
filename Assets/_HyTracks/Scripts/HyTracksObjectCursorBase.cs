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
		public RectTransform uiInputSocial;
		public RectTransform uiInputTechnology;
		public RectTransform uiInputEconomic;
		public RectTransform uiInputEnvirontment;
		public RectTransform uiInputPolitics;

		[Header("Dynamic Panels")]
		[SerializeField]
		DynamicPanelsCanvas dpcSoc;
		[SerializeField]
		DynamicPanelsCanvas dpcTec;
		[SerializeField]
		DynamicPanelsCanvas dpcEco;
		[SerializeField]
		DynamicPanelsCanvas dpcEnv;
		[SerializeField]
		DynamicPanelsCanvas dpcPol;

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

		void InitUI() {

			PanelNotificationCenter.OnStartedDraggingTab += PanelNotificationCenter_OnStartedDraggingTab;
			PanelNotificationCenter.OnStoppedDraggingTab += PanelNotificationCenter_OnStoppedDraggingTab;

			foreach (STEEPDimension dim in Enum.GetValues(typeof(STEEPDimension))) {

				DynamicPanelsCanvas dpc = null;
				RectTransform uiParent = null;
				Sprite sprite = null;
				switch (dim)
				{
					case STEEPDimension.SOCIAL:
						uiParent = uiInputSocial;
						dpc = dpcSoc;
						sprite = themeSettings.themeSoc.sprite;
						break;
					case STEEPDimension.TECHNOLOGY:
						uiParent = uiInputTechnology;
						dpc = dpcTec;
						sprite = themeSettings.themeTec.sprite;
						break;
					case STEEPDimension.ECONOMICS:
						uiParent = uiInputEconomic;
						dpc = dpcEco;
						sprite = themeSettings.themeEco.sprite;
						break;
					case STEEPDimension.ENVIRONMENT:
						uiParent = uiInputEnvirontment;
						dpc = dpcEnv;
						sprite = themeSettings.themeEnv.sprite;
						break;
					case STEEPDimension.POLITICS:
						uiParent = uiInputPolitics;
						dpc = dpcPol;
						sprite = themeSettings.themePol.sprite;
						break;
				}
				HyTracksParametersList paramList = GetInputParameters(dim);
				//BuildParametersForDimension(dim, paramList, uiParent, dpc, sprite);


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


