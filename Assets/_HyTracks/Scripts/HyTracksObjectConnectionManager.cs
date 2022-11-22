using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HyTracks
{
    public class HyTracksObjectConnectionManager : MonoBehaviour
    {
        public HyTracksCursorManager cursorManager;

		public HyTracksSteepConnections steepConnections;
        public HyTracksObjectConnectionList connections;

		[EditorCools.Button]
		void LoadConnections()
		{
			steepConnections.LoadDataFromJSONFiles();
			connections = steepConnections.connections;
		}

		private void OnEnable()
		{
			LoadConnections();
			cursorManager.onTangibleAdded += CursorManager_onTangibleAdded;
			cursorManager.onTangibleRemoved += CursorManager_onTangibleRemoved;
			cursorManager.onTangibleUpdated += CursorManager_onTangibleUpdated;

		}

		
		private void CursorManager_onTangibleAdded(HyTracksObjectCursorBase obj)
		{
			Debug.Log(obj);
			Debug.Log("CursorManager_onTangibleAdded");
		}
		private void CursorManager_onTangibleRemoved(HyTracksObjectCursorBase obj)
		{
			Debug.Log("CursorManager_onTangibleRemoved");
		}
		private void CursorManager_onTangibleUpdated(HyTracksObjectCursorBase obj)
		{
			Debug.Log("CursorManager_onTangibleUpdated");
		}


		private void OnDisable()
		{
			cursorManager.onTangibleAdded -= CursorManager_onTangibleAdded;
			cursorManager.onTangibleRemoved -= CursorManager_onTangibleRemoved;
			cursorManager.onTangibleUpdated -= CursorManager_onTangibleUpdated;
		}

		// Start is called before the first frame update
		void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
