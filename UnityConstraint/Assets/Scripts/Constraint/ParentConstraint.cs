using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constraint;

//[ExecuteInEditMode]
public class ParentConstraint : ConstraintBase
{
    [SerializeField]
    protected bool isOffset;
    [SerializeField]
    protected bool isScaleOffset;
    [SerializeField]
    protected float offsetAmount = 1.0f;

    private void Start ()
    {
        ApplyParentConstraint();
    }
	
	private void Update ()
    {
		if((objTarget != null) && !isOneShot)
        {
            ApplyParentConstraint();
        }
	}

    public void ApplyParentConstraint()
    {
        // Apply rotation.
        //Quaternion rotT = objTarget.transform.rotation;
        //transform.rotation = rotT;
        //Quaternion rotT = GetRotDiff(Vector3.up, objTarget.transform.forward);
        //transform.rotation = rotT * objTarget.transform.rotation;
        Quaternion rotT = objTarget.transform.rotation;
        transform.rotation = rotT * rotOffset;

        // Apply position.
        Vector3 posT = objTarget.transform.position;
        Vector3 posM = Vector3.zero;
        if (isOffset)
        {
            Vector3 offsetFixed = offset * offsetAmount;
            Vector3 sclDiff = GetSclDiff();
            if(isScaleOffset)
            {
                offsetFixed = new Vector3(offsetFixed.x * sclDiff.x, offsetFixed.y * sclDiff.y, offsetFixed.z * sclDiff.z);
            }
            posM = posT + objTarget.transform.rotation * offsetFixed;
        }
        else
        {
            posM = posT;
        }
        transform.position = posM;       
    }
}
