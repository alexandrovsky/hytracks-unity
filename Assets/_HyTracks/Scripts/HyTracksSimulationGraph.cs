using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace HyTracks
{
	[CreateAssetMenu(menuName = "HyTracks/SimulationGraph")]
	public class HyTracksSimulationGraph : NodeGraph
    {
        [Range(0,1)]
        public float simulationTime;
    }
}
