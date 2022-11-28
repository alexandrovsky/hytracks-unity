using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace HyTracks
{
    [CreateAssetMenu(menuName = "HyTracks/SteepParameters")]
    public class HyTracksSteepParameters:HyTracksSteepData {
        public DateTime date;
        public string title = "Bau Scenario 2022";

        [Header("Input Parameters Text Files")]
        public TextAsset parametersSocialInputJSON;
        public TextAsset parametersTechnologyInputJSON;        
        public TextAsset parametersEconomicsInputJSON;
        public TextAsset parametersEnvironmentInputJSON;
        public TextAsset parametersPoliticsInputJSON;

        [Header("Output Parameters Text Files")]
        public TextAsset parametersSocialOutputJSON;
        public TextAsset parametersTechnologyOutputJSON;
        public TextAsset parametersEconomicsOutputJSON;
        public TextAsset parametersEnvironmentOutputJSON;
        public TextAsset parametersPoliticsOutputJSON;


		[Header("SOCIAL")]
		[Space(10)]
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersSocialInput;
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersSocialOutput;
		[Space(10)]
		[Header("TECHNOLOGY")]
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersTechnologyInput;
		[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersTechnologyOutput;
		[Space(10)]
		[Header("ECONOMICS")]
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersEconomicsInput;
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersEconomicsOutput;
		[Space(10)]
		[Header("ENVIRONMENT")]
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersEnvironmentInput;
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersEnvironmentOutput;
		[Space(10)]
		[Header("POLITICS")]
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersPoliticsInput;
		//[HideInInspector]
		public HyTracksParametersList<HyTracksParametersBase> parametersPoliticsOutput;


		private SerializedProperty parametersProps;

		public Dictionary<STEEPDimension, HyTracksParametersList<HyTracksParametersBase>> parametersInput { private set; get; }
		public Dictionary<STEEPDimension, HyTracksParametersList<HyTracksParametersBase>> parametersOutput { private set; get; }



		private void OnEnable()
        {
			LoadDataFromJSONFiles();
        }

		public HyTracksParametersList<HyTracksParametersBase> GetParameters(string dimension, HyTracksParametersType type)
        {

			STEEPDimension steep = STEEPDimension.SOCIAL;
			if (dimension.ToLower().Contains("soc"))
            {
                steep = STEEPDimension.SOCIAL;
			}
            else if (dimension.ToLower().Contains("tec"))
			{
				steep = STEEPDimension.TECHNOLOGY;
			}
            else if (dimension.ToLower().Contains("eco"))
			{
				steep = STEEPDimension.ECONOMICS;
			}
			else if (dimension.ToLower().Contains("env"))
			{
				steep = STEEPDimension.ENVIRONMENT;
			}
			else if (dimension.ToLower().Contains("pol"))
			{
				steep = STEEPDimension.POLITICS;
			}

            return GetParameters(steep, type);
		}

		public HyTracksParametersList<HyTracksParametersBase> GetParameters(STEEPDimension steep, HyTracksParametersType type)
		{
            if(type == HyTracksParametersType.INPUT)
            {
                if (parametersInput.ContainsKey(steep))
                {
					return parametersInput[steep];
				}
				else
				{
					return null;
				}
			}
            else
            {
				if (parametersOutput.ContainsKey(steep))
                {
					return parametersOutput[steep];
                }
                else
                {
                    return null;
                }
					
			}
		}

		


		[EditorCools.Button]
		public override void LoadDataFromJSONFiles()
        {
            
			// INPUTS
			if (parametersSocialInputJSON != null) {                
				//parametersSocialInput = JsonUtility.FromJson<HyTracksParametersList>(parametersSocialInputJSON.text);
				parametersSocialInput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersSocialInputJSON.text);
			}
            if(parametersTechnologyInputJSON != null) {
                parametersTechnologyInput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersTechnologyInputJSON.text);
            }
            if(parametersEconomicsInputJSON != null) {
                parametersEconomicsInput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersEconomicsInputJSON.text);
            }
            if(parametersEnvironmentInputJSON != null) {
                parametersEnvironmentInput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersEnvironmentInputJSON.text);
            }
            if(parametersPoliticsInputJSON != null) {
                parametersPoliticsInput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersPoliticsInputJSON.text);
            }

            // OUTPUTS
            if (parametersSocialOutputJSON != null)
            {
                parametersSocialOutput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersSocialOutputJSON.text);
            }
            if (parametersTechnologyOutputJSON != null)
            {
                parametersTechnologyOutput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersTechnologyOutputJSON.text);
            }
            if (parametersEconomicsOutputJSON != null)
            {
                parametersEconomicsOutput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersEconomicsOutputJSON.text);
            }
            if (parametersEnvironmentOutputJSON != null)
            {
                parametersEnvironmentOutput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersEnvironmentOutputJSON.text);
            }
            if (parametersPoliticsOutputJSON != null)
            {
                parametersPoliticsOutput = HyTracksParametersList<HyTracksParametersBase>.BuildFromJSON(parametersPoliticsOutputJSON.text);
            }

			GenerateDicts();
		}

		void GenerateDicts()
		{
			parametersInput = new Dictionary<STEEPDimension, HyTracksParametersList<HyTracksParametersBase>>() {
				{STEEPDimension.SOCIAL, parametersSocialInput },
				{STEEPDimension.TECHNOLOGY, parametersTechnologyInput},
				{STEEPDimension.ECONOMICS, parametersEconomicsInput},
				{STEEPDimension.ENVIRONMENT, parametersEnvironmentInput},
				{STEEPDimension.POLITICS, parametersPoliticsInput},
			};

			parametersOutput = new Dictionary<STEEPDimension, HyTracksParametersList<HyTracksParametersBase>>() {
				{STEEPDimension.SOCIAL, parametersSocialOutput},
				{STEEPDimension.TECHNOLOGY, parametersTechnologyOutput},
				{STEEPDimension.ECONOMICS, parametersEconomicsOutput},
				{STEEPDimension.ENVIRONMENT, parametersEnvironmentOutput},
				{STEEPDimension.POLITICS, parametersPoliticsOutput},
			};
		}

        [Multiline]
        [SerializeField]
        string dummyJSON;

        [EditorCools.Button]
        public void GenerateDummyJSONParamters() {
            HyTracksParametersBase p = new HyTracksParametersBase()
            {
                id = "test_id",
                name = "test_name",
                unit = "m/s^2",
                description = "description text....",
		        value = 42,
		        isVisible = true,
		        isEditable = true,
				date = DateTime.Now.ToString("yyyy-MM-dd")
			};
			dummyJSON = JsonUtility.ToJson(p);
            Debug.Log(dummyJSON);
        }

		[EditorCools.Button]
		public void GenerateDummyJSONParamtersList()
		{
            HyTracksParametersList<HyTracksParametersBase> pList = new HyTracksParametersList<HyTracksParametersBase>();
            pList.parameters = new List<HyTracksParametersBase>()
            {
                new HyTracksParametersBase() {
                    id = "test_id_0",
                    name = "test_name",
                    value = 0.0f,
                    unit = "m/s^2",
                    date = DateTime.Now.ToString("yyyy-MM-dd")
                },
				new HyTracksParametersBase() {
				    id = "test_id_1",
				    name = "test_name",
				    value = 1.0f,
				    unit = "mW",
				    date = DateTime.Now.ToString("yyyy-MM-dd")
			    }
			};

			

			pList.parameters.Add(new HyTracksParametersBase()
			);

			dummyJSON = JsonUtility.ToJson(pList);
			Debug.Log(dummyJSON);
		}

	}
}
