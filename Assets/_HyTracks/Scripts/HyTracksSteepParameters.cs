using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

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
        public HyTracksParametersList parametersSocialInput;
        public HyTracksParametersList parametersSocialOutput;
        [Space(10)]
        [Header("TECHNOLOGY")]
        public HyTracksParametersList parametersTechnologyInput;
        public HyTracksParametersList parametersTechnologyOutput;
        [Space(10)]
        [Header("ECONOMICS")]
        public HyTracksParametersList parametersEconomicsInput;
        public HyTracksParametersList parametersEconomicsOutput;
        [Space(10)]
        [Header("ENVIRONMENT")]
        public HyTracksParametersList parametersEnvironmentInput;
        public HyTracksParametersList parametersEnvironmentOutput;
        [Space(10)]
        [Header("POLITICS")]
        public HyTracksParametersList parametersPoliticsInput;
        public HyTracksParametersList parametersPoliticsOutput;


		public Dictionary<STEEPDimension, HyTracksParametersList> parametersInput { private set; get; }
		public Dictionary<STEEPDimension, HyTracksParametersList> parametersOutput { private set; get; }



		private void OnEnable()
        {
			LoadDataFromJSONFiles();
        }

		public HyTracksParametersList GetParameters(string dimension, HyTracksParametersType type)
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

		public HyTracksParametersList GetParameters(STEEPDimension steep, HyTracksParametersType type)
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
                parametersSocialInput = JsonUtility.FromJson<HyTracksParametersList>(parametersSocialInputJSON.text);
            }
            if(parametersTechnologyInputJSON != null) {
                parametersTechnologyInput = JsonUtility.FromJson<HyTracksParametersList>(parametersTechnologyInputJSON.text);
            }
            if(parametersEconomicsInputJSON != null) {
                parametersEconomicsInput = JsonUtility.FromJson<HyTracksParametersList>(parametersEconomicsInputJSON.text);
            }
            if(parametersEnvironmentInputJSON != null) {
                parametersEnvironmentInput = JsonUtility.FromJson<HyTracksParametersList>(parametersEnvironmentInputJSON.text);
            }
            if(parametersPoliticsInputJSON != null) {
                parametersPoliticsInput = JsonUtility.FromJson<HyTracksParametersList>(parametersPoliticsInputJSON.text);
            }

            // OUTPUTS
            if (parametersSocialOutputJSON != null)
            {
                parametersSocialOutput = JsonUtility.FromJson<HyTracksParametersList>(parametersSocialOutputJSON.text);
            }
            if (parametersTechnologyOutputJSON != null)
            {
                parametersTechnologyOutput = JsonUtility.FromJson<HyTracksParametersList>(parametersTechnologyOutputJSON.text);
            }
            if (parametersEconomicsOutputJSON != null)
            {
                parametersEconomicsOutput = JsonUtility.FromJson<HyTracksParametersList>(parametersEconomicsOutputJSON.text);
            }
            if (parametersEnvironmentOutputJSON != null)
            {
                parametersEnvironmentOutput = JsonUtility.FromJson<HyTracksParametersList>(parametersEnvironmentOutputJSON.text);
            }
            if (parametersPoliticsOutputJSON != null)
            {
                parametersPoliticsOutput = JsonUtility.FromJson<HyTracksParametersList>(parametersPoliticsOutputJSON.text);
            }

			GenerateDicts();
		}

		void GenerateDicts()
		{
			parametersInput = new Dictionary<STEEPDimension, HyTracksParametersList>() {
				{STEEPDimension.SOCIAL, parametersSocialInput },
				{STEEPDimension.TECHNOLOGY, parametersTechnologyInput},
				{STEEPDimension.ECONOMICS, parametersEconomicsInput},
				{STEEPDimension.ENVIRONMENT, parametersEnvironmentInput},
				{STEEPDimension.POLITICS, parametersPoliticsInput},
			};

			parametersOutput = new Dictionary<STEEPDimension, HyTracksParametersList>() {
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
        void GenerateDummyJSONParamters() {
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
		void GenerateDummyJSONParamtersList()
		{
            HyTracksParametersList pList = new HyTracksParametersList();
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
