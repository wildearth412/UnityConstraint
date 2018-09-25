using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constraint;

public class WildScaleConstraint : WildConstraintBase
{
    [SerializeField]
    private bool isScaleOffset = true;      // If false, it will duplicate target's scale.
    [SerializeField]
    private bool isInvert = false;          // If true, it will invert target's scale.
    private Vector3 sclOrgMyself;

    private void Start ()
    {
        sclOrgMyself = transform.localScale;
        ApplyScaleConstraint();
    }
	
	private void Update ()
    {
        if ((objTarget != null) && !isOneShot)
        {
            ApplyScaleConstraint();
        }
    }

    public void ApplyScaleConstraint()
    {
        // Apply scale.
        if(isScaleOffset)
        {
            if (!isInvert)
            {
                Vector3 sclDiff = GetSclDiff();
                sclDiff = new Vector3(sclDiff.x * sclOrgMyself.x, sclDiff.y * sclOrgMyself.y, sclDiff.z * sclOrgMyself.z);
                transform.localScale = sclDiff;
            }
            else
            {
                Vector3 sclDiff = GetSclDiff();
                sclDiff = new Vector3(1.0f / sclDiff.x * sclOrgMyself.x, 1.0f / sclDiff.y * sclOrgMyself.y, 1.0f / sclDiff.z * sclOrgMyself.z);
                transform.localScale = sclDiff;
            }
        }
        else
        {
            if(!isInvert)
            {
                transform.localScale = objTarget.transform.lossyScale;
            }
            else
            {
                Vector3 sclDiff = GetSclDiff();
                sclDiff = new Vector3(1.0f / sclDiff.x * sclOrgTarget.x, 1.0f / sclDiff.y * sclOrgTarget.y, 1.0f / sclDiff.z * sclOrgTarget.z);
                transform.localScale = sclDiff;
            }
        }
    }
}
