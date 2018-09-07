using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constraint;

public class WildPositionConstraint : WildParentConstraint
{
    [SerializeField]
    private bool constraintX = true;
    [SerializeField]
    private bool constraintY = true;
    [SerializeField]
    private bool constraintZ = true;

	private void Start ()
    {
        ApplyPositionConstraint();
    }
	
	private void Update ()
    {
        if ((objTarget != null) && !isOneShot)
        {
            ApplyPositionConstraint();
        }        
    }

    public void ApplyPositionConstraint()
    {
        // Apply position.
        Vector3 posT = objTarget.transform.position;
        Vector3 posM = Vector3.zero;
        if (isOffset)
        {
            Vector3 offsetFixed = offset * offsetAmount;
            Vector3 sclDiff = GetSclDiff();
            if (isScaleOffset)
            {
                offsetFixed = new Vector3(offsetFixed.x * sclDiff.x, offsetFixed.y * sclDiff.y, offsetFixed.z * sclDiff.z);
            }
            posM = posT + offsetFixed;
            posM = new Vector3(constraintX ? posM.x : transform.position.x, constraintY ? posM.y : transform.position.y, constraintZ ? posM.z : transform.position.z);
        }
        else
        {
            posM = posT;
        }

        transform.position = posM;
    }
}
