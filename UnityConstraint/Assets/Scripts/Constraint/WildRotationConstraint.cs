using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constraint;

public enum RotationType
{
    All,                  // About xyz-axis
    Yaw,                  // About z-axis
    Pitch,                // About y-axis
    Roll,                 // About x-axis
}

public class WildRotationConstraint : WildConstraintBase
{
    [SerializeField]
    private RotationType rotationType = RotationType.All;
    [SerializeField]
    private bool isRotationOffset = true;      // If false, it will duplicate target's rotation.

    void Start ()
    {
        ApplyRotationConstraint();
    }
	
	void Update ()
    {
        if ((objTarget != null) && !isOneShot)
        {
            ApplyRotationConstraint();
        }
    }

    public void ApplyRotationConstraint()
    {
        // Apply rotation.
        switch(rotationType)
        {
            case RotationType.All:
                SetRotation();
                break;
            case RotationType.Yaw:
                SetRotation();
                Vector3 vy = new Vector3(transform.up.x,transform.up.y,0);
                LookAt(Vector3.forward,vy);
                break;
            case RotationType.Pitch:
                SetRotation();
                Vector3 vp = new Vector3(transform.forward.x, 0, transform.forward.z);
                LookAt(vp,Vector3.up);
                break;
            case RotationType.Roll:
                SetRotation();
                Vector3 vru = new Vector3(0,transform.right.y, transform.right.z);
                Quaternion r = Quaternion.FromToRotation(Vector3.up,Vector3.forward);
                Vector3 vrf = r * vru;
                LookAt(vrf, vru);
                break;
        }
    }

    private void SetRotation()
    {
        Quaternion rotT = objTarget.transform.rotation;
        if (isRotationOffset)
        {
            transform.rotation = rotT * rotOffset;
        }
        else
        {
            transform.rotation = rotT;
        }
    }
}
