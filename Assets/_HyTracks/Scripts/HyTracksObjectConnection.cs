using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TouchScript.Behaviors.Cursors;
namespace HyTracks
{
    [System.Serializable]
    public class HyTracksObjectConnectionData
    {
        public int id;
        public int inputObjectID;
		public int outputObjectID;
        public string inputParameterID;
		public string outputParameterID;
	}


	[System.Serializable]
	public struct HyTracksObjectConnectionList
	{
		public List<HyTracksObjectConnectionData> connections;
	}


	public class HyTracksObjectConnection : MonoBehaviour
	{
		public RectTransform rect;
		public HyTracksObjectConnectionData connectionData;
		public UILineConnector connector;
		public UILineRenderer lineRenderer;
	}
}
