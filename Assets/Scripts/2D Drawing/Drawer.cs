/*
 * Drawer.cs
 * Written by Fidel Soto
 * Implementation borrowed from and extended upon https://www.youtube.com/watch?v=sHE5ubsP-E8
 * 
 * Takes care of drawing/erasing on objects with the "Drawable" tag and 
 * are either on the "Drawboard" or "Notecard" Layers by applying pixels to the texture of 
 * the board.
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

    [SerializeField]
    private Transform tip;
    private float tipHeight;

    [SerializeField]
    private int penSize = 5;


    private Renderer r;
    private Color[] colors;

    private RaycastHit touch;
    private Drawable whiteboard;
    
    private Vector2 touchPos, lastTouchPos;
    private bool touchedLastFrame;
    private Quaternion lastTouchRot;

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

        if ((Physics.Raycast(tip.position, transform.right, out touch, tipHeight, LayerMask.GetMask("NoteCard"))
                || Physics.Raycast(tip.position, transform.right, out touch, tipHeight, LayerMask.GetMask("DrawBoard")))
            && touch.transform.CompareTag("Drawable") && GetComponent<DrawController3D>().trigger.action.ReadValue<float>() == 0.0f)
        {

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
            // of the notecard when drawing is happening.
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

            // Instantaneous movement makes it so that the Drawer pen ignores physics. So while 
            // we're drawing, change the movement type to velocity tracking so that the pen accounts
            // for physics and doesn't clip through the board.
            xrGrabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
            xrGrabInteractable.smoothPosition = true;
            xrGrabInteractable.smoothPositionAmount = 16f;
            xrGrabInteractable.smoothRotation = true;

            if (touchedLastFrame && (isDeleting || drawMode.action.ReadValue<float>() > 0.0f))
            {
                whiteboard.SetPixels(x, y, penSize, penSize, colors);

                // Interpolate between the previous pixels that were set and the current ones.
                for (float f = 0.01f; f < 1.00; f += 0.01f)
                {
                    var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
                    var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);
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
        else
        {
            // Ensure that if we're not detecting any drawing being done, 
            xrGrabInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
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

}
