using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyTracks
{
    [System.Serializable]
    public class HyTracksObjectConnection
    {
        public int id;
        public int inputObjectID;
		public int outputObjectID;
        public string inputParameterID;
		public string outputParameterID;
	}
}
