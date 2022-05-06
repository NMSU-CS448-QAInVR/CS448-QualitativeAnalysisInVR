/*
 * Drawer.cs
 * Written by Fidel Soto
 * Implementation borrowed from and extended upon https://www.youtube.com/watch?v=sHE5ubsP-E8
 * 
 * Takes care of drawing/erasing on objects with the "Drawable" tag and 
 * are either on the "Drawboard" or "Notecard" Layers by applying pixels to the texture of 
 * the board.
 * 
 * When the pen detects that a raycast has hit a drawable object (either a notecard or a board), 
 * then the pen will shot out a laser letting the user know where the drawing/erasing will land. 
 * 
 * When holding the pen, the user will press B to draw, and A to delete.
 * 
 * Erasing, in actuality, is just drawing the color of the board. (default to white)
 * 
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Drawer : MonoBehaviour
{
    private XRGrabInteractable xrGrabInteractable;

    // If on delete mode, the pen will be drawing white pixels
    public InputActionReference deleteMode;
    private bool isDeleting = false;

    // If on draw mode, the pen will be drawing red pixels
    public InputActionReference drawMode;

    // The tip that we'll be tracking. The tip will shoot out the raycast and the laser.
    [SerializeField]
    private Transform tip;
    private float tipHeight;

    // How big the drawings should be
    [SerializeField]
    private int penSize = 5;

    // Necessary to render the material
    private Renderer r;
    private Color[] colors;

    // Information about the touch on the whiteboard when the raycast is hit
    private RaycastHit touch;
    private Drawable whiteboard;
    
    // Coordinates of the current position, and previous position if we were drawing in the previous
    // frame
    private Vector2 touchPos, lastTouchPos;
    private bool touchedLastFrame;
    private Quaternion lastTouchRot;

    public Material lineMat;
    // Start is called before the first frame update
    void Start()
    {
        r = tip.GetComponent<Renderer>();
        colors = Enumerable.Repeat(Color.red, penSize * penSize).ToArray();
        tipHeight = .4f;
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        // Every frame, check if we're either deleting or drawing, and then continue with the
        // actual drawing logic.
        CheckDeleteState();
        Draw();
    }

    private void Draw()
    {
        // If the raycast hit a notecard or a board, then draw a line to that drawable object and if the user is pressing A or B, 
        // then go ahead and draw or delete.
        if ((Physics.Raycast(tip.position, transform.right, out touch, Mathf.Infinity, LayerMask.GetMask("NoteCard"))
                || Physics.Raycast(tip.position, transform.right, out touch, Mathf.Infinity, LayerMask.GetMask("DrawBoard")))
            && touch.transform.CompareTag("Drawable") && GetComponent<DrawController3D>().trigger.action.ReadValue<float>() == 0.0f)
        {
            DrawLine(tip.position, touch.point, Color.blue);
            if (whiteboard == null)
            {
                whiteboard = touch.transform.GetComponent<Drawable>();
            }

            // If we switched from one drawable to another drawable,
            // set the current whiteboard to the new drawable and reset touchedLastFrame
            // so that we don't draw on this frame
            if (whiteboard.transform.name != touch.transform.name || whiteboard.gameObject != touch.transform.gameObject)
            {
                // Before switching to the next drawable, ensure that the current drawable's box collider is 
                // enabled
                if (whiteboard.GetComponent<BoxCollider>() != null)
                {
                    whiteboard.GetComponent<BoxCollider>().enabled = true;
                }

                whiteboard = touch.transform.GetComponent<Drawable>();
                touchedLastFrame = false;
            }

            // Notecards have a BoxCollider and a MeshCollider. 
            // Refactoring them such that their prefabs match the board prefabs
            // was too complex, so for now I'm opting to just toggle the box collider
            // off the notecard when drawing is happening.
            if (touch.transform.name.Contains("Notecard"))
            {
                whiteboard.transform.GetComponent<BoxCollider>().enabled = false;
            }

            // Get the coordinates of the touchPosition. If they are out of bounds, return.
            touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);

            int x = (int)(touchPos.x * whiteboard.textureSize.x - (penSize / 2));
            int y = (int)(touchPos.y * whiteboard.textureSize.y - (penSize / 2));

            if ((y < 0 || y > whiteboard.textureSize.y) ||
                    (x < 0 || x > whiteboard.textureSize.x)
                )
            {
                return;
            }

            // If the board was touched in the previous frame, and we are currently either 
            // drawing or deleting, then begin setting pixels appropiately
            if (touchedLastFrame && (isDeleting || drawMode.action.ReadValue<float>() > 0.0f))
            {
                whiteboard.SetPixels(x, y, penSize, penSize, colors);

                // Interpolate between the previous pixels that were set and the current ones.
                for (float f = 0.01f; f < 1.00; f += 0.01f)
                {
                    var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
                    var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);

                    // When erasing, the pixel set is bigger so that it's not tedious to erase
                    if (isDeleting)
                    {
                        whiteboard.SetPixels(lerpX, lerpY, penSize * 15, penSize * 15, colors);
                    }
                    else
                    {
                        whiteboard.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                    }
                }

                whiteboard.texture.Apply();
            }

            lastTouchPos = new Vector2(x, y);
            lastTouchRot = transform.rotation;
            touchedLastFrame = true;
            return;

        }

        // Always ensure that the boxCollider is re-enabled when we're done drawing 
        // (this is for the notecard)
        if (whiteboard?.transform.GetComponent<BoxCollider>() != null)
        {
            whiteboard.transform.GetComponent<BoxCollider>().enabled = true;
        }

        whiteboard = null;
        touchedLastFrame = false;

    }

    // Check whether we're deleting or drawing. 
    // This is done by checking whether the A or B button is being pressed.
    private void CheckDeleteState()
    {
        isDeleting = deleteMode.action.ReadValue<float>() > 0.0f;
        if (isDeleting && whiteboard != null)
        {
            colors = Enumerable.Repeat(Color.white, (penSize * 15) * (penSize * 15)).ToArray();
        }
        else
        {
            colors = Enumerable.Repeat(Color.red, penSize * penSize).ToArray();
        }
    }

    // Create a line from the pen to the board by using a line renderer on a new 
    // game object. The color holds no bearing at the moment, as the material of the line
    // will always be green. After that, delete the line during this frame, as it can't move.
    // The next frame, if the pen is still raycast hitting the board, then make another. 
    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.01f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = lineMat;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

}
