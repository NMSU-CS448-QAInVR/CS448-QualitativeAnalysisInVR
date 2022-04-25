using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DrawingSaveFormat : SaveFormat
{
    // location
    public float x;
    
    public float y;
    
    public float z;

    // scale
    public float x_scale;
    public float y_scale;
    public float z_scale;

    //
    public float[] x_values;
    public float[] y_values;
    public float[] z_values;

    public DrawingSaveFormat() : base(FormatType.DRAWING) {
    } // end NotecardSaveFormat

    public DrawingSaveFormat(GameObject drawing) : base(FormatType.DRAWING) {
        Transform transform = drawing.transform;
        if (transform == null) {
            throw new Exception("Cannot find transform component in the game object");
        } // end if
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        x_scale = transform.localScale.x;
        y_scale = transform.localScale.y;
        z_scale = transform.localScale.z;

        LineRenderer lr = drawing.GetComponent<LineRenderer>();
        Vector3[] points = new Vector3[lr.positionCount];
        int actualCount = lr.GetPositions(points);

        x_values = new float[actualCount];
        y_values = new float[actualCount];
        z_values = new float[actualCount];
        for (int i = 0; i < actualCount; ++i) {
            x_values[i] = points[i].x;
            y_values[i] = points[i].y;
            z_values[i] = points[i].z;
        } // end for i
    } // end NotecardSaveFormat

    public override void LoadObjectInto(GameObject draw3DController) {
        DrawController3D dc3 = draw3DController.GetComponent<DrawController3D>();
        Vector3[] pointsOnLine = new Vector3[x_values.Length];
        for (int i = 0; i < pointsOnLine.Length; ++i) {
            pointsOnLine[i].x = x_values[i];
            pointsOnLine[i].y = y_values[i];
            pointsOnLine[i].z = z_values[i];
        } // end for i

        GameObject obj = new GameObject("temp");
        obj.transform.position = new Vector3(x, y, z);
        dc3.LoadLineForSaveSystem(pointsOnLine, obj);
        GameObject.Destroy(obj);
    } // end LoadObject
}
