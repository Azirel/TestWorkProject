using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GestureRecognizer;

public class CursorManagerScript : MonoBehaviour
{
    public LineRenderer line;
    class TracePoint
    {
        public Vector3 point;
        public float lifeTimne;
        public TracePoint(Vector3 v3,float time)
        {
            point = v3;
            this.lifeTimne = time;
        }
    }
    public float pointsLifeTime;
    List<TracePoint> points;
    Vector3 pos;
    public float startThincknes;
    public float endThincknes;
    void Start()
    {
        points = new List<TracePoint>();
        line.SetWidth(startThincknes, endThincknes);
    }
    void FixedUpdate()
    {
        if (Utility.IsTouchDevice() || Input.GetMouseButton(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            points.Insert(0, new TracePoint(pos, pointsLifeTime));
        }
        LastPointsEliminator();
        if(points.Count>1)
        {
            line.enabled = true;
        }
        ModifyLine();
    }
    void ModifyLine()
    {
        line.SetVertexCount(points.Count);
        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i].point);
        }
    }
    void LastPointsEliminator()
    {
        foreach (var point in points)
        {
            point.lifeTimne -= Time.deltaTime;
        }
        for (int i = points.Count-1; i >-1; i--)
        {
            if(points[i].lifeTimne<=0f)
            {
                points.RemoveAt(i);
            }
        }
    }
}
