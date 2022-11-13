using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript.Behaviors.Cursors;


namespace HyTracks {
    public class HyTracksObjectCursorBase:ObjectCursor {

		public HyTracksSteepParameters parameters;


        public HyTracksParametersList GetInputParameters(STEEPDimension steep)
		{
			return parameters.parametersInput[steep];
		}

		public HyTracksParametersList GetOutputParameters(STEEPDimension steep)
		{
			return parameters.parametersOutput[steep];
		}

	}

}


