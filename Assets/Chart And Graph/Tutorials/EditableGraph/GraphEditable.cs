#define Graph_And_Chart_PRO
using ChartAndGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEditable : MonoBehaviour
{
    public GraphChart Graph;
    public string Category;
    public RectTransform LastPoint;
    List<DoubleVector3> mDataArray = new List<DoubleVector3>();
    const double MaxDistSqr = 0.025 * 0.025;
    int mMovingPoint = -1;
    int mLastMovingPoint = -1;
    void Start()
    { 
        if (Graph != null)
        {
            Graph.OnRedraw.AddListener(Redraw);
        }
    }

    void Redraw()
    {
        if (mLastMovingPoint < 0)
        {
            if (LastPoint != null)
            {
                if (LastPoint.gameObject.activeSelf)
                    LastPoint.gameObject.SetActive(false);
            }
        }
        else
        {
            var last = mDataArray[mLastMovingPoint];
            Vector3 pos;
            if (Graph.PointToWorldSpace(out pos, last.x, last.y, Category))
            {
                if (LastPoint != null)
                {
                    if (LastPoint.gameObject.activeSelf == false)
                        LastPoint.gameObject.SetActive(true);
                    LastPoint.transform.position = pos;
                }
            }
        }
    }

    int FindNearPoint(DoubleVector3 position)
    {
        int minDist = -1;
        double currentMinDist = double.PositiveInfinity;
        for(int i=0; i<mDataArray.Count; i++)
        {
            DoubleVector3 diff = mDataArray[i] - position;
            diff.x /= Graph.DataSource.HorizontalViewSize;
            diff.y /= Graph.DataSource.VerticalViewSize;
            double sqrDist = (diff.x * diff.x) + (diff.y * diff.y);

            if(sqrDist< MaxDistSqr && sqrDist<currentMinDist)
            {
                currentMinDist = sqrDist;
                minDist = i;
            }
        }
        return minDist;
    }
    int InsertPoint(DoubleVector3 point)
    {
        int index = 0;
        for(; index < mDataArray.Count; index++)
        {
            if (point.x <= mDataArray[index].x)
                break;
        }
        mDataArray.Insert(index, point);
        return index;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        double x, y;
        Graph.PointToClient(mousePos, out x, out y);
        DoubleVector3 mouseChartPos = new DoubleVector3(x, y, 0);
        if (Input.GetMouseButtonDown(0))
        {
            mMovingPoint = FindNearPoint(mouseChartPos);
            if(mMovingPoint == -1)
                mMovingPoint = InsertPoint(mouseChartPos);
            mLastMovingPoint = mMovingPoint;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mMovingPoint = -1;
        }
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            if(mLastMovingPoint >= 0)
            {
                mDataArray.RemoveAt(mLastMovingPoint);
                mLastMovingPoint = -1;
                mMovingPoint = -1;
            }
        }
        if(mMovingPoint != -1)
        {
            DoubleVector3 ClampedMousePos = mouseChartPos;
            if (mMovingPoint > 0)
                ClampedMousePos.x = Math.Max(mDataArray[mMovingPoint - 1].x,ClampedMousePos.x);
            if (mMovingPoint + 1 < mDataArray.Count)
                ClampedMousePos.x = Math.Min(mDataArray[mMovingPoint + 1].x, ClampedMousePos.x);
            mDataArray[mMovingPoint] = ClampedMousePos;
        }
        Graph.DataSource.SetCategoryArray(Category, mDataArray, 0, mDataArray.Count);
    }
}
