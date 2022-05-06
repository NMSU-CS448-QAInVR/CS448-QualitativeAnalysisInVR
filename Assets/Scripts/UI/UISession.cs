using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIController;

/*
    A UI session that will keep track of the history of submenus moving.
*/
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

    /*
        Move to a new sub menu. 
        Input: the new menu to add to.
        Output: the input sub menu.
    */
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

    /*
        Move to the forward menu.
        Output: the destination sub menu.
    */
    public BaseSubMenuController MoveToForwardMenu() {
        BaseSubMenuController temp = forward.Dequeue();
        if (temp == null)
            return temp;

        prev.Push(current);
        current = temp;
        return current;
    } // end GetForward

    /*
        Move to the previous menu.
        Output: the destination sub menu.
    */
    public BaseSubMenuController MoveToPrevMenu() {
        BaseSubMenuController temp = prev.Pop();
        if (temp == null)
            return temp;
            
        forward.Enqueue(current);
        current = temp;
        return current;
    } // GetPrev

    /*
        Get the current menu.
        Output: the current sub menu.
    */
    public BaseSubMenuController GetCurrent() {
        return current;
    } // GetCurrent

}
