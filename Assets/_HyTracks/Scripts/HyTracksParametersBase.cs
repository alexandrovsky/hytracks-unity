using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GraphAndChartSimpleJSON;
using System.ComponentModel;

namespace HyTracks {

    public enum STEEPDimension {
        SOCIAL,
        TECHNOLOGY,
        ECONOMICS,
        ENVIRONMENT,
        POLITICS
	}

	public enum HyTracksParametersType
    {
        INPUT,
        OUTPUT
    }

	[Serializable]
    public class HyTracksParametersBase {
        public string id;
        public string name;
        public string unit;
        [MultilineAttribute]
        public string description;
        public float value;
        public float minValue;
        public float maxValue;
        public bool isVisible;
        public bool isEditable;
        public string date;

        public void ParseJSON(JSONNode json)
        {
            this.id = json["id"];
            this.name = json["name"];
			this.unit = json["unit"];
            this.description = json["description"];
            this.value = json["value"];
            this.minValue = json["minValue"];
			this.maxValue = json["maxValue"];
            this.isVisible = json["isVisible"];
			this.isVisible = json["isVisible"];
			this.isEditable = json["isEditable"];
			this.date = json["date"];
		}
    }

    [Serializable]
    public class HyTracksParametersCategory : HyTracksParametersBase
    {
		[Browsable(false)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new float value;
        public int selectedValueIndex;
        public string[] values;
    }

	[Serializable]
    public class HyTracksParametersList<T> where T: HyTracksParametersBase
	{
        public List<HyTracksParametersBase> parameters;

        public HyTracksParametersBase GetParameters(string id)
        {
            return parameters.Where(x => x.id == id).FirstOrDefault();

        }

        public static HyTracksParametersList<T> BuildFromJSON(string jsonData)
        {
            HyTracksParametersList<T> parametersList = new HyTracksParametersList<T>();
            parametersList.parameters = new List<HyTracksParametersBase>();

			JSONNode json = JSON.Parse(jsonData);
			 
			foreach(JSONNode pNode in json["parameters"])
            {
                Debug.Log(pNode);
                HyTracksParametersBase parameters = null;
                string parameterType = pNode["parameterType"];
				switch (parameterType)
				{                    
                    case "category":
                        parameters = new HyTracksParametersCategory();
                        break;
					default:
						parameters = new HyTracksParametersBase();
						break;
				}

				parameters.ParseJSON(pNode);
                parametersList.parameters.Add(parameters);
			}

            return parametersList;
		}
    }

}
