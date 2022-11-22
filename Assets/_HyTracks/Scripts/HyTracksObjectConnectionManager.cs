using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TouchScript.Utils;
using TouchScript.Behaviors.Cursors;
using System;
namespace HyTracks
{
    public class HyTracksObjectConnectionManager : MonoBehaviour
    {
        public HyTracksCursorManager cursorManager;

		public HyTracksSteepConnections steepConnections;
        public HyTracksObjectConnectionList connections;

		public HyTracksObjectConnection connectionPrefab;

		private ObjectPool<HyTracksObjectConnection> connectionPool;

		[SerializeField]
		List<HyTracksObjectConnection> openConnections;


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

			List<HyTracksObjectConnectionData> conns = connections.connections.Where(x => x.inputObjectID == obj.objectId || x.outputObjectID == obj.objectId).ToList();

			foreach (HyTracksObjectConnectionData data in conns)
			{
				bool otherIsInput = data.inputObjectID != obj.objectId;
				int otherId = data.inputObjectID == obj.objectId ? data.outputObjectID : data.inputObjectID;
				if (cursorManager.cursors.ContainsKey(otherId))
				{
					var connection = connectionPool.Get();
					connection.connectionData = data;

					if (otherIsInput)
					{
						connection.connector.transforms.Append(cursorManager.cursors[otherId].transform);
						connection.connector.transforms.Append(this.transform);
					}
					else
					{
						connection.connector.transforms.Append(this.transform);
						connection.connector.transforms.Append(cursorManager.cursors[otherId].transform);
					}

					connection.connector.enabled = true;
					connection.rect.SetParent(this.transform);
					connection.rect.SetAsLastSibling();

					openConnections.Add(connection);					
				}
			}
		}
		private void CursorManager_onTangibleRemoved(HyTracksObjectCursorBase obj)
		{
			Debug.Log("CursorManager_onTangibleRemoved");
			List<HyTracksObjectConnection> conns = openConnections.Where(x => x.connectionData.inputObjectID == obj.objectId || x.connectionData.outputObjectID == obj.objectId).ToList();
			foreach(var c in conns)
			{
				connectionPool.Release(c);
				openConnections.Remove(c);
			}
			
			
		}
		private void CursorManager_onTangibleUpdated(HyTracksObjectCursorBase obj)
		{
			//Debug.Log("CursorManager_onTangibleUpdated");
		}


		private void OnDisable()
		{
			cursorManager.onTangibleAdded -= CursorManager_onTangibleAdded;
			cursorManager.onTangibleRemoved -= CursorManager_onTangibleRemoved;
			cursorManager.onTangibleUpdated -= CursorManager_onTangibleUpdated;
		}

		private HyTracksObjectConnection instantiateConnectionProxy()
		{
			return Instantiate(connectionPrefab);
		}

		private void clearProxy(HyTracksObjectConnection connection)
		{
			connection.connector.enabled = false;
		}

		private void Awake()
		{
			connectionPool = new ObjectPool<HyTracksObjectConnection>(10, instantiateConnectionProxy, null, clearProxy);
			openConnections = new List<HyTracksObjectConnection>();
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
