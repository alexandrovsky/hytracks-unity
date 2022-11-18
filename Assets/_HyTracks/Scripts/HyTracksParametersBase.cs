using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyTracks {

    public enum STEEPDimension {
        SOCIAL,
        TECHNOLOGY,
        ECONOMICS,
        ENVIRONMENT,
        POLITICS
	}

    [Serializable]
    public struct HyTracksParametersBase {
        public string id;
        public string name;
        public string unit;
        [MultilineAttribute]
        public string description;
        public float value;
        public bool isVisible;
        public bool isEditable;
    }


    [Serializable]
    public struct HyTracksParametersList {
        public List<HyTracksParametersBase> parameters;
        
    }

}
