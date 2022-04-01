using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIController;

public class UISession
{
    Stack<BaseSubMenuController> prev;
    Queue<BaseSubMenuController> forward;
    BaseSubMenuController current;

    public UISession(BaseSubMenuController intial) {
        prev = new Stack<BaseSubMenuController>();
        forward = new Queue<BaseSubMenuController>();  
        current = intial;
    } // end UISession

    public BaseSubMenuController MoveToNewMenu(BaseSubMenuController menu) {
        if (menu == null) {
            Debug.LogError("Cannot add null menu");
            return null;
        } // end if  
        prev.Push(current);
        forward.Clear();
        current = menu;
        return current;
    } // end PushMenu

    public BaseSubMenuController MoveToForwardMenu() {
        BaseSubMenuController temp = forward.Dequeue();
        if (temp == null)
            return temp;

        prev.Push(current);
        current = temp;
        return current;
    } // end GetForward

    public BaseSubMenuController MoveToPrevMenu() {
        BaseSubMenuController temp = prev.Pop();
        if (temp == null)
            return temp;
            
        forward.Enqueue(current);
        current = temp;
        return current;
    } // GetPrev

    public BaseSubMenuController GetCurrent() {
        return current;
    } // GetCurrent

}
