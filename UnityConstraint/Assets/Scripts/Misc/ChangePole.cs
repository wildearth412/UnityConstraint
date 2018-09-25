using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePole : MonoBehaviour
{
    public WildDistanceConstraint dc;
    public GameObject obj1;
    public GameObject obj2;

	public void ChangePoleObj(int i)
    {
        switch(i)
        {
            case 1:
                dc.poleTarget = obj1;
                break;
            case 2:
                dc.poleTarget = obj2;
                break;
        }
    }
}
