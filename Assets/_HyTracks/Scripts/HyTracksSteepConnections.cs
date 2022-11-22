using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyTracks
{
	[CreateAssetMenu(menuName = "HyTracks/SteepConnections")]
	public class HyTracksSteepConnections : HyTracksSteepData
    {

		[Header("Connections Text File")]
		public TextAsset connectionsJSON;



		[Space(30)]
		[Header("Connections")]
		public HyTracksObjectConnectionList connections;
		
		[EditorCools.Button]
		public override void LoadDataFromJSONFiles()
		{
			// CONNECTIONS 
			if (connectionsJSON != null)
			{
				connections = JsonUtility.FromJson<HyTracksObjectConnectionList>(connectionsJSON.text);
			}
		}
	}
}
