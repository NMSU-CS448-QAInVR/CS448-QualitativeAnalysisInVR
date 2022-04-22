using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Drawer : MonoBehaviour
{
    private XRGrabInteractable xrGrabInteractable;
    public InputActionReference deleteMode;
    private bool isDeleting = false;

    [SerializeField]
    private Transform tip;

    [SerializeField]
    private int penSize = 5;

    private Renderer r;
    private Color[] colors;
    private float tipHeight;

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
        CheckDeleteState();
        Draw();
    }

    private void Draw()
    {

        if (Physics.Raycast(tip.position, transform.right, out touch, tipHeight)
            && touch.transform.CompareTag("Drawable") && GetComponent<DrawController3D>().trigger.action.ReadValue<float>() == 0.0f)
        {
            if (touch.transform.CompareTag("Drawable"))
            {
                Debug.Log("Hit!");
                if (whiteboard == null)
                {
                    whiteboard = touch.transform.GetComponent<Drawable>();
                }

                touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);

                int x = (int)(touchPos.x * whiteboard.textureSize.x - (penSize / 2));
                int y = (int)(touchPos.y * whiteboard.textureSize.y - (penSize / 2));

                if ( (y < 0 || y > whiteboard.textureSize.y) ||
                     (x < 0 || x > whiteboard.textureSize.x)
                   )
                {
                    return;
                }

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                xrGrabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
                xrGrabInteractable.smoothPosition = true;
                xrGrabInteractable.smoothPositionAmount = 16f;
                xrGrabInteractable.smoothRotation = true;

                if (touchedLastFrame)
                {
                    whiteboard.texture.SetPixels(x, y, penSize, penSize, colors);

                    // Interpolation
                    for (float f = 0.01f; f < 1.00; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);
                        if (isDeleting)
                        {
                            whiteboard.texture.SetPixels(lerpX, lerpY, penSize * 15, penSize * 15, colors);
                        }
                        else
                        {
                            whiteboard.texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                        }
                    }


                    // transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    whiteboard.texture.Apply();
                }

                lastTouchPos = new Vector2(x, y);
                lastTouchRot = transform.rotation;
                touchedLastFrame = true;
                return;

            }
            
        }
        else
        {
            xrGrabInteractable.movementType = XRBaseInteractable.MovementType.Instantaneous;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
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
