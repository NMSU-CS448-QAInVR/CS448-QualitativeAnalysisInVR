using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawController3D : MonoBehaviour
{
    public bool isErasing = false;
    public bool isGrabbed = false;
    private GameObject go;
    // Action reference to trigger the 3d draw functionality
    // (set in the editor). Should be a trigger press.
    public InputActionReference trigger = null;

    public InputActionReference deleteMode = null;

    // Object whose position will be tracked when drawing. 
    // This should be the tip of a brush or pen or something. 
    // Testing to see if tracking an empty object at the end of a pen
    // would work.
    public GameObject gameObjectToTrack;

    [SerializeField, Range(0, 1.0f)]
    private float minDistanceBeforeNewPoint = 0.01f;

    private int positionCount = 0;

    // Store lines in a list. Could be useful when loading/saving
    // a session.
    private List<LineRenderer> lines = new List<LineRenderer>();
    private LineRenderer currentLineRender;

    [SerializeField, Range(0, 1.0f)]
    private float lineDefaultWidth = 0.02f;

    [SerializeField]
    private Material defaultLineMaterial;

    private bool isTriggerReleased = false;

    private Vector3 prevPointDistance = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        CheckTriggerState();
        CheckDeleteState();
    }

    void Awake()
    {
        AddNewLineRenderer();
    }

    void AddNewLineRenderer()
    {
        positionCount = 0;

        go = new GameObject($"LineRenderer_brush_{lines.Count}");
        // go.transform.parent = gameObjectToTrack.transform.parent;
        go.transform.position = gameObjectToTrack.transform.position;

        LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
        goLineRenderer.startWidth = lineDefaultWidth;
        goLineRenderer.endWidth = lineDefaultWidth;
        goLineRenderer.useWorldSpace = true;
        goLineRenderer.material = defaultLineMaterial;
        goLineRenderer.positionCount = 1;
        goLineRenderer.numCapVertices = 360;

        currentLineRender = goLineRenderer;

        lines.Add(goLineRenderer);
    }

    void CheckTriggerState()
    {
        bool isTriggerPressing = trigger.action.ReadValue<float>() > 0.1f;

        if (isTriggerPressing && isGrabbed)
        {
            UpdateLine();
            isTriggerReleased = true;
            return;
        }

        if (isTriggerReleased)
        {
            AddNewLineRenderer();
            isTriggerReleased = false;
        }

    }

    void CheckDeleteState()
    {
        bool isPrimaryButtonPressed = deleteMode.action.ReadValue<float>() > 0.0f;

        if (isPrimaryButtonPressed && isGrabbed)
        {
            isErasing = true;
        }
        else
        {
            isErasing = false;
        }

    }

    void UpdateLine()
    {
        if (prevPointDistance == null)
        {
            prevPointDistance = gameObjectToTrack.transform.position;
        }

        if (prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, gameObjectToTrack.transform.position)) >= minDistanceBeforeNewPoint)
        {
            prevPointDistance = gameObjectToTrack.transform.position;
            AddPoint(prevPointDistance);
        }

    }

    void AddPoint(Vector3 position)
    {
        currentLineRender.SetPosition(positionCount, position);
        positionCount++;
        currentLineRender.positionCount = positionCount + 1;
        currentLineRender.SetPosition(positionCount, position);

        var colliderGameObject = new GameObject();
        colliderGameObject.transform.position = position;
        colliderGameObject.transform.parent = go.transform;
        colliderGameObject.tag = "LineCollider";
        AddCollider(colliderGameObject);
    }

    void AddCollider(GameObject gameObject)
    {
        SphereCollider sc = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        sc.radius = lineDefaultWidth * .25f;
    }

    public void UpdateIsGrabbed(bool state)
    {
        isGrabbed = state;
    }


}
