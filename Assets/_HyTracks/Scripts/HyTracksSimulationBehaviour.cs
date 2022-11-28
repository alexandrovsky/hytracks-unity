using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyTracks
{
    public class HyTracksSimulationBehaviour : SingletonBehaviour<HyTracksSimulationBehaviour>
    {
        [Range(0,1)]
        public float simulationTime;

        public HyTracksSimulationGraph simulationGraph;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            foreach (HyTracksObjectCursorBase tangible in HyTracksCursorManager.instance.tangibles.Values)
            {

            }
        }


        public void SetSimulationTime(float t)
        {
            simulationTime = t;

		}
    }
}
