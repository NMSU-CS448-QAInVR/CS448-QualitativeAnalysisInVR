using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAction : MonoBehaviour
{   
    public GameObject CardPrefab;
    public Color CardColor;
    public Color CategoryColor;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateCard() {
        Object.Instantiate(CardPrefab, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
    } // end CreateCard

}
