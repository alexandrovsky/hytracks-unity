using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DynamicPanels;
using static TouchScript.Behaviors.Cursors.UI.GradientTexture;

namespace HyTracks
{
    public class HyTracksParametersUI : MonoBehaviour
    {
        [SerializeField]
        TMP_Text nameText;

        [SerializeField]
        TMP_Text descriptionText;

        [SerializeField]
        TMP_InputField valueInputField;

        [SerializeField]
        TMP_Text unitsText;

        [SerializeField]
        RectTransform inputPanel;

		[SerializeField]
		Button incrementBtn;
		[SerializeField]
		Button decrementBtn;

		DynamicPanelsCanvas dynamicPanelsCanvas;

		public HyTracksParametersBase parameters { get; private set;}

        public void Init(HyTracksParametersBase parameters, DynamicPanelsCanvas dpc, HyTracksThemeSettings themeSettings)
		{
            this.parameters = parameters;
            nameText.text = parameters.name;
            descriptionText.text = parameters.description;
            unitsText.text = parameters.unit;
            valueInputField.text = $"{parameters.value}";
            valueInputField.interactable = parameters.isEditable;
            valueInputField.onValueChanged.AddListener((s) => {
				if(float.TryParse(s,out float res)) {
                    parameters.value = res;
                }
            });

            if(parameters.minValue == parameters.maxValue)
            {
                parameters.maxValue = parameters.value;
			}

            incrementBtn.onClick.AddListener(() => {
				parameters.value += themeSettings.inrementStep;                
				valueInputField.text = $"{parameters.value}";
			});

			decrementBtn.onClick.AddListener(() => {
				parameters.value -= themeSettings.inrementStep;
				valueInputField.text = $"{parameters.value}";
			});

            inputPanel.gameObject.SetActive(parameters.isEditable);

			dynamicPanelsCanvas = dpc;
        }


        public void RealignToOriginalDynamicPanel() {
            PanelTab tab = PanelUtils.GetAssociatedTab(this.transform as RectTransform);
            (dynamicPanelsCanvas.RootPanelGroup[0] as Panel).AddTab(tab);
        }


		// Start is called before the first frame update
		void Start()
        {
            


        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
