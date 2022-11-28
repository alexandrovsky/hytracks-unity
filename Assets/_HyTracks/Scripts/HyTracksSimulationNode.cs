using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;
using System.Linq;

namespace HyTracks
{
    public class HyTracksSimulationNode : Node
    {
		[Input] public float time;

		public List<HyTracksSteepParameters> parameters;

		public new void OnEnable()
		{
			base.OnEnable();
			ClearDynamicPorts();
			foreach (STEEPDimension dim in Enum.GetValues(typeof(STEEPDimension)))
			{
				for(int i = 0; i < parameters.Count; i++)
				{
					// INPUTS
					var parametersList = parameters[i].GetParameters(dim, HyTracksParametersType.INPUT);
					if (parametersList != null)
					{
						var inputParams = parametersList.parameters;
						foreach (var p in inputParams)
						{
							if (DynamicPorts.Where(x => x.fieldName == p.id).ToList().Count == 0)
							{
								NodePort newPort = AddDynamicInput(typeof(float), fieldName: p.id);
							}
						}
					}
					// OUTPUTS
					parametersList = parameters[i].GetParameters(dim, HyTracksParametersType.OUTPUT);
					if(parametersList != null)
					{
						var outputParams = parametersList.parameters;
						foreach (var p in outputParams)
						{
							if (DynamicPorts.Where(x => x.fieldName == p.id).ToList().Count == 0)
							{
								NodePort newPort = AddDynamicOutput(typeof(float), fieldName: p.id);
							}
						}
					}
					
				}				
			}
		}

		public override object GetValue(NodePort port)
		{
			
			HyTracksSteepParameters steep = parameters[(int)(time == 1.0 ? parameters.Count - 1 : time * parameters.Count)];
			HyTracksParametersList<HyTracksParametersBase> parametersList;
			if (port.IsInput)
			{
				parametersList = steep.GetParameters(port.fieldName, HyTracksParametersType.INPUT);				
			}
			else
			{
				parametersList = steep.GetParameters(port.fieldName, HyTracksParametersType.OUTPUT);				
			}
			return parametersList.parameters.Where(x => x.id == port.fieldName).FirstOrDefault().value;
			//return base.GetValue(port);
		}
	}
}
