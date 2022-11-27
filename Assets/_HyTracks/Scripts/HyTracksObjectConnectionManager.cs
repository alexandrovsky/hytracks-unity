using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TouchScript.Utils;
using TouchScript.Behaviors.Cursors;
using System;
namespace HyTracks
{
    public class HyTracksObjectConnectionManager : SingletonBehaviour<HyTracksObjectConnectionManager>
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
			//Debug.Log(obj);
			//Debug.Log("CursorManager_onTangibleAdded");

			List<HyTracksObjectConnectionData> conns = connections.connections.Where(x => x.inputObjectID == obj.objectId || x.outputObjectID == obj.objectId).ToList();

			foreach (HyTracksObjectConnectionData data in conns)
			{
				bool otherIsInput = data.inputObjectID != obj.objectId;
				int otherId = data.inputObjectID == obj.objectId ? data.outputObjectID : data.inputObjectID;
				if (cursorManager.tangibles.ContainsKey(otherId))
				{
					var connection = connectionPool.Get();
					
					connection.connectionData = data;
					connection.connector.enabled = true;
					connection.connector.transforms = new RectTransform[2];
					var otherTransform = cursorManager.tangibles[otherId].transform as RectTransform;
					if (otherIsInput)
					{
						connection.connector.transforms[0] = otherTransform;						
						connection.connector.transforms[1] = obj.transform as RectTransform;
					}
					else
					{
						connection.connector.transforms[0] = obj.transform as RectTransform; 
						connection.connector.transforms[1] = otherTransform;
					}

					
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
			return Instantiate(connectionPrefab, this.transform);
		}

		private void getConnectionProxy(HyTracksObjectConnection connection)
		{
			connection.gameObject.SetActive(true);
		}

		private void clearProxy(HyTracksObjectConnection connection)
		{
			connection.gameObject.SetActive(false);
			//connection.connector.enabled = false;
		}

		private new void Awake()
		{
			base.Awake();
			connectionPool = new ObjectPool<HyTracksObjectConnection>(10, instantiateConnectionProxy, getConnectionProxy, clearProxy);
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
