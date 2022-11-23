using System.Collections.Generic;
using TouchScript.Utils;
using TouchScript.Pointers;
using TouchScript.Utils.Attributes;
using UnityEngine;
using UnityEngine.Profiling;
using TouchScript.Behaviors.Cursors;
using TouchScript;
using System;


namespace HyTracks {


	[System.Serializable]
	public class HyTracksCursorEntry<T> where T: HyTracksObjectCursorBase {
		public string name;
		public int tuioID;
		public T cursor;
	}

	public class HyTracksCursorManager:MonoBehaviour {

		#region Public properties

		public delegate void HyTracksTanbgileEventHandler(HyTracksObjectCursorBase obj);

		public event HyTracksTanbgileEventHandler onTangibleAdded;
		public event HyTracksTanbgileEventHandler onTangibleRemoved;
		public event HyTracksTanbgileEventHandler onTangibleUpdated;

		/// <summary>
		/// Prefab to use as mouse cursors template.
		/// </summary>
		public PointerCursor MouseCursor {
			get { return mouseCursor; }
			set { mouseCursor = value; }
		}

		/// <summary>
		/// Prefab to use as touch cursors template.
		/// </summary>
		public PointerCursor TouchCursor {
			get { return touchCursor; }
			set { touchCursor = value; }
		}

		/// <summary>
		/// Prefab to use as pen cursors template.
		/// </summary>
		public PointerCursor PenCursor {
			get { return penCursor; }
			set { penCursor = value; }
		}

		/// <summary>
		/// Prefab to use as object cursors template.
		/// </summary>
		public ObjectCursor ObjectCursor {
			get { return objectCursor; }
			set { objectCursor = value; }
		}

		/// <summary>
		/// Gets or sets whether <see cref="CursorManager"/> is using DPI to scale pointer cursors.
		/// </summary>
		/// <value> <c>true</c> if DPI value is used; otherwise, <c>false</c>. </value>
		public bool UseDPI {
			get { return useDPI; }
			set {
				useDPI = value;
				updateCursorSize();
			}
		}

		/// <summary>
		/// Gets or sets the size of pointer cursors in cm. This value is only used when <see cref="UseDPI"/> is set to <c>true</c>.
		/// </summary>
		/// <value> The size of pointer cursors in cm. </value>
		public float CursorSize {
			get { return cursorSize; }
			set {
				cursorSize = value;
				updateCursorSize();
			}
		}

		/// <summary>
		/// Cursor size in pixels.
		/// </summary>
		public uint CursorPixelSize {
			get { return cursorPixelSize; }
			set {
				cursorPixelSize = value;
				updateCursorSize();
			}
		}

		#endregion

		#region Private variables

		[SerializeField]
		private bool cursorsProps; // Used in the custom inspector

		[SerializeField]
		private PointerCursor mouseCursor;

		[SerializeField]
		private PointerCursor touchCursor;

		[SerializeField]
		private PointerCursor penCursor;

		[SerializeField]
		private ObjectCursor objectCursor;


		[SerializeField]
		private List<HyTracksCursorEntry<HyTracksObjectCursorModel>> modelObjectCursors;
		

		[SerializeField]
		private List<HyTracksCursorEntry<HyTracksObjectCursorAgent>> agentObjectCursors;



		[ToggleLeft]
		private bool useDPI = true;

		[SerializeField]
		private float cursorSize = 1f;

		[SerializeField]
		private uint cursorPixelSize = 64;

		public RectTransform rect { get; private set; }
		private ObjectPool<PointerCursor> mousePool;
		private ObjectPool<PointerCursor> touchPool;
		private ObjectPool<PointerCursor> penPool;
		private ObjectPool<ObjectCursor> objectPool;

		private Dictionary<int, ObjectPool<HyTracksObjectCursorBase>> hyTracksObjectsPool;
		

		public Dictionary<int, PointerCursor> cursors { get; private set; }
		public Dictionary<int, PointerCursor> tangibles { get; private set; }

		private CustomSampler cursorSampler;

		#endregion

		#region Unity methods

		private void Awake()
		{
			cursors = new Dictionary<int, PointerCursor>(10);
			tangibles = new Dictionary<int, PointerCursor>(10);
			cursorSampler = CustomSampler.Create("[TouchScript] Update Cursors");

			cursorSampler.Begin();

			mousePool = new ObjectPool<PointerCursor>(2,instantiateMouseProxy,null,clearProxy);
			touchPool = new ObjectPool<PointerCursor>(10,instantiateTouchProxy,null,clearProxy);
			penPool = new ObjectPool<PointerCursor>(2,instantiatePenProxy,null,clearProxy);
			objectPool = new ObjectPool<ObjectCursor>(2,instantiateObjectProxy,null,clearProxy);


			hyTracksObjectsPool = new Dictionary<int, ObjectPool<HyTracksObjectCursorBase>>();
			


			foreach (HyTracksCursorEntry<HyTracksObjectCursorModel> entry in modelObjectCursors)
			{
				var pool = new ObjectPool<HyTracksObjectCursorBase>(2, () => {
					var obj = Instantiate(entry.cursor);
					obj.name = $"{entry.cursor.name}_{entry.tuioID}";
					return obj as HyTracksObjectCursorBase;
				}, null, clearProxy);
				hyTracksObjectsPool.Add(entry.tuioID, pool);
			}

			foreach (HyTracksCursorEntry<HyTracksObjectCursorAgent> entry in agentObjectCursors)
			{
				var pool = new ObjectPool<HyTracksObjectCursorBase>(2, () => {
					var obj = Instantiate(entry.cursor);
					obj.name = $"{entry.cursor.name}_{entry.tuioID}";
					return obj as HyTracksObjectCursorBase;
				}, null, clearProxy);
				hyTracksObjectsPool.Add(entry.tuioID, pool);
			}

			updateCursorSize();

			rect = transform as RectTransform;
			if(rect == null) {
				Debug.LogError("CursorManager must be on an UI element!");
				enabled = false;
			}

			cursorSampler.End();
		}

		private void OnEnable()
		{
			if(TouchManager.Instance != null) {
				TouchManager.Instance.PointersAdded += pointersAddedHandler;
				TouchManager.Instance.PointersRemoved += pointersRemovedHandler;
				TouchManager.Instance.PointersPressed += pointersPressedHandler;
				TouchManager.Instance.PointersReleased += pointersReleasedHandler;
				TouchManager.Instance.PointersUpdated += PointersUpdatedHandler;
				TouchManager.Instance.PointersCancelled += pointersCancelledHandler;
			}
		}

		private void OnDisable()
		{
			if(TouchManager.Instance != null) {
				TouchManager.Instance.PointersAdded -= pointersAddedHandler;
				TouchManager.Instance.PointersRemoved -= pointersRemovedHandler;
				TouchManager.Instance.PointersPressed -= pointersPressedHandler;
				TouchManager.Instance.PointersReleased -= pointersReleasedHandler;
				TouchManager.Instance.PointersUpdated -= PointersUpdatedHandler;
				TouchManager.Instance.PointersCancelled -= pointersCancelledHandler;
			}
		}

		#endregion

		#region Private functions

		private PointerCursor instantiateMouseProxy()
		{
			return Instantiate(mouseCursor);
		}

		private PointerCursor instantiateTouchProxy()
		{
			return Instantiate(touchCursor);
		}

		private PointerCursor instantiatePenProxy()
		{
			return Instantiate(penCursor);
		}

		private ObjectCursor instantiateObjectProxy()
		{
			return Instantiate(objectCursor);
		}

		private void clearProxy(PointerCursor cursor)
		{
			cursor.Hide();
		}

		private void updateCursorSize()
		{
			if(useDPI) cursorPixelSize = (uint)(cursorSize * TouchManager.Instance.DotsPerCentimeter);
		}

		#endregion

		#region Event handlers

		private void pointersAddedHandler(object sender,PointerEventArgs e)
		{
			cursorSampler.Begin();

			updateCursorSize();

			var count = e.Pointers.Count;
			for(var i = 0; i < count; i++) {
				var pointer = e.Pointers[i];
				// Don't show internal pointers
				if((pointer.Flags & Pointer.FLAG_INTERNAL) > 0) continue;

				PointerCursor cursor;
				switch(pointer.Type) {
					case Pointer.PointerType.Mouse:
						cursor = mousePool.Get();
						break;
					case Pointer.PointerType.Touch:
						cursor = touchPool.Get();
						break;
					case Pointer.PointerType.Pen:
						cursor = penPool.Get();
						break;
					case Pointer.PointerType.Object:
						// TODO: update to the tangible dict
						int objectId = (pointer as ObjectPointer).ObjectId;

						if (hyTracksObjectsPool.ContainsKey(objectId))
						{
							cursor = hyTracksObjectsPool[objectId].Get();
						}						
						else {
							cursor = objectPool.Get();
						}

						(cursor as HyTracksObjectCursorBase).objectId = objectId;

						if (tangibles.ContainsKey(objectId))
						{
							tangibles[objectId] = cursor;
						}
						else
						{
							tangibles.Add(objectId, cursor);
						}

						onTangibleAdded.Invoke(cursor as HyTracksObjectCursorBase);
						break;
					default:
						continue;
				}

				cursor.Size = cursorPixelSize;
				cursor.Init(rect,pointer);
				cursors.Add(pointer.Id,cursor);
			}

			cursorSampler.End();
		}

		private void pointersRemovedHandler(object sender,PointerEventArgs e)
		{
			cursorSampler.Begin();

			var count = e.Pointers.Count;
			for(var i = 0; i < count; i++) {
				var pointer = e.Pointers[i];
				PointerCursor cursor;
				if(!cursors.TryGetValue(pointer.Id,out cursor)) continue;
				cursors.Remove(pointer.Id);

				switch(pointer.Type) {
					case Pointer.PointerType.Mouse:
						mousePool.Release(cursor);
						break;
					case Pointer.PointerType.Touch:
						touchPool.Release(cursor);
						break;
					case Pointer.PointerType.Pen:
						penPool.Release(cursor);
						break;
					case Pointer.PointerType.Object:
						
						onTangibleRemoved.Invoke(cursor as HyTracksObjectCursorBase);
						
						var hyCursor = cursor as HyTracksObjectCursorBase;

						if (hyTracksObjectsPool.ContainsKey(hyCursor.objectId))
						{
							hyTracksObjectsPool[hyCursor.objectId].Release(cursor);
						}
						else
						{
							objectPool.Release(cursor);
						}
						
						tangibles.Remove((cursor as HyTracksObjectCursorBase).objectId);
						break;
				}
			}

			cursorSampler.End();
		}

		private void pointersPressedHandler(object sender,PointerEventArgs e)
		{
			cursorSampler.Begin();

			var count = e.Pointers.Count;
			for(var i = 0; i < count; i++) {
				var pointer = e.Pointers[i];
				PointerCursor cursor;
				if(!cursors.TryGetValue(pointer.Id,out cursor)) continue;
				cursor.SetState(pointer,PointerCursor.CursorState.Pressed);
			}

			cursorSampler.End();
		}

		private void PointersUpdatedHandler(object sender,PointerEventArgs e)
		{
			cursorSampler.Begin();

			var count = e.Pointers.Count;
			for(var i = 0; i < count; i++) {
				var pointer = e.Pointers[i];
				PointerCursor cursor;
				if(!cursors.TryGetValue(pointer.Id,out cursor)) continue;
				cursor.UpdatePointer(pointer);


				switch (pointer.Type)
				{
					case Pointer.PointerType.Mouse:						
						break;
					case Pointer.PointerType.Touch:						
						break;
					case Pointer.PointerType.Pen:						
						break;
					case Pointer.PointerType.Object:
						// TODO: update to the tangible dict
						onTangibleUpdated.Invoke(cursor as HyTracksObjectCursorBase);
						break;
				}

			}

			cursorSampler.End();
		}

		private void pointersReleasedHandler(object sender,PointerEventArgs e)
		{
			cursorSampler.Begin();

			var count = e.Pointers.Count;
			for(var i = 0; i < count; i++) {
				var pointer = e.Pointers[i];
				PointerCursor cursor;
				if(!cursors.TryGetValue(pointer.Id,out cursor)) continue;
				cursor.SetState(pointer,PointerCursor.CursorState.Released);
			}

			cursorSampler.End();
		}

		private void pointersCancelledHandler(object sender,PointerEventArgs e)
		{
			pointersRemovedHandler(sender,e);
		}

		#endregion
	}
}