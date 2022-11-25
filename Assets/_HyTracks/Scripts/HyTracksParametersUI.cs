using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DynamicPanels;
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

        DynamicPanelsCanvas dynamicPanelsCanvas;

		public HyTracksParametersBase parameters { get; private set;}

        public void Init(HyTracksParametersBase parameters, DynamicPanelsCanvas dpc)
		{
            this.parameters = parameters;
            nameText.text = parameters.name;
            descriptionText.text = parameters.description;
            unitsText.text = parameters.unit;
            valueInputField.text = $"{parameters.value}";
            valueInputField.onValueChanged.AddListener((s) => {
				if(float.TryParse(s,out float res)) {
                    parameters.value = res;
                }
            });

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
