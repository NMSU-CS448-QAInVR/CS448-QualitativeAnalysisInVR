using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISession
{
    Stack<GameObject> prev;
    Queue<GameObject> forward;
    GameObject current;

    public UISession(GameObject intial) {
        prev = new Stack<GameObject>();
        forward = new Queue<GameObject>();  
        current = intial;
    } // end UISession

    public GameObject MoveToNewMenu(GameObject menu) {
        if (menu == null) {
            Debug.LogError("Cannot add null menu");
            return null;
        } // end if  
        prev.Push(current);
        forward.Clear();
        current = menu;
        return current;
    } // end PushMenu

    public GameObject MoveToForwardMenu() {
        GameObject temp = forward.Dequeue();
        if (temp == null)
            return temp;

        prev.Push(current);
        current = temp;
        return current;
    } // end GetForward

    public GameObject MoveToPrevMenu() {
        GameObject temp = prev.Pop();
        if (temp == null)
            return temp;
            
        forward.Enqueue(current);
        current = temp;
        return current;
    } // GetPrev

    public GameObject GetCurrent() {
        return current;
    } // GetCurrent

}
