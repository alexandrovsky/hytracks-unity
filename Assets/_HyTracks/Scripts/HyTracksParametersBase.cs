using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    }


    [Serializable]
    public class HyTracksParametersList {
        public List<HyTracksParametersBase> parameters;

  //      public HyTracksParametersBase GetParameters(string id)
  //      {
  //          return parameters.Where(x => x.id == id).FirstOrDefault();

		//}
        
    }

}
