using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderMoving : MonoBehaviour
{
    #region Fields
    public GameObject character = null;
    public RuntimeAnimatorController animatorController = null;
    private Animator charAnimator = null;

    [Tooltip("Just show it for debug,don't change it in inspector!")]
    [SerializeField]
    private bool isWanderMoving = true;
    [Tooltip("Just show it for debug,don't change it in inspector!")]
    [SerializeField]
    private bool isWaiting = false;
    public float speedFactor = 1.0f;

    [Tooltip("The time that animals wait at station.")]
    public float waitAtStation = 15.0f;
    public float waitIntervalMin = 5.0f;
    public float waitIntervalMax = 7.0f;
    [Tooltip("Just show it for debug,don't change it in inspector!")]
    [SerializeField]
    private float waitIntervalNow = 0.0f;
    public float stopProbability = 0.3f;

    public float speedMin = 0.8f;
    public float speedMax = 2.5f;
    [Tooltip("max rotation angle per second (0-360)")]
    public float angleMax = 30.0f;       // max rotation angle per second (0-360)

    [Tooltip("Just show it for debug,don't change it in inspector!")]
    [SerializeField]
    private float lineVelocityNow = 0.0f;
    [SerializeField]
    private float lineVelocityNext = 0.0f;
    private float angularVelocity = 0.0f;
    private float localRotationY = 0.0f;

    [Tooltip("Just show it for debug,don't change it in inspector!")]
    [SerializeField]
    private bool isGettingBack;
    private Vector3 targetDirection;    // For changing direction of moving.

    #endregion

    private void Awake ()
    {
        charAnimator = character.GetComponent<Animator>();
        charAnimator.runtimeAnimatorController = animatorController;
        localRotationY = this.transform.rotation.eulerAngles.y;
        isGettingBack = false;
    }

    private void Update ()
    {
        if (isWanderMoving)
        {
            Moving();
        }
	}

    private void Moving()
    {
        waitIntervalNow -= Time.deltaTime;

        if (!isGettingBack)
        {
            if (waitIntervalNow < 0f)
            {
                // At sometime, for this animal it is not necessary to keep waiting, but the"isWaiting" status is still true. We need to fix it.
                if (isWaiting == true) { isWaiting = false; }

                waitIntervalNow = Random.Range(waitIntervalMin,waitIntervalMax);
                if(Random.value < stopProbability)
                {
                    lineVelocityNow = 0.0f;
                    lineVelocityNext = 0.0f;
                }
                else
                {
                    lineVelocityNext = lineVelocityNow = Random.Range(speedMin,speedMax);
                    angularVelocity = Random.Range(-angleMax,angleMax);
                }
            }

            if (lineVelocityNow > 0.1f)
            {
                localRotationY += angularVelocity * Time.deltaTime;
                this.transform.rotation = Quaternion.Euler(0, localRotationY, 0);
            }
            lineVelocityNow = (lineVelocityNext * 0.1f) + (lineVelocityNow * 0.9f);
            this.transform.position += (transform.rotation * new Vector3(0, 0, lineVelocityNow * speedFactor)) * Time.deltaTime;
        }
        else
        {
            if (waitIntervalNow < 0)
            {
                // At sometime, for this animal it is not necessary to keep waiting, but the"isWaiting" status is still true. We need to fix it.
                if (isWaiting == true) { isWaiting = false; }

                isGettingBack = false;
            }
            
            if(targetDirection != transform.forward)
            {
                Vector3 stepDir = Vector3.RotateTowards(transform.forward, targetDirection, lineVelocityNext * Time.deltaTime, 0);
                this.transform.rotation = Quaternion.LookRotation(stepDir);
                localRotationY = this.transform.rotation.eulerAngles.y;
            }

            lineVelocityNow = (lineVelocityNext * 0.05f) + (lineVelocityNow * 0.95f);
            this.transform.position += (transform.rotation * new Vector3(0, 0, lineVelocityNow * speedFactor)) * Time.deltaTime;
        }

        if (charAnimator != null)
        {
            SetAnimator(lineVelocityNow);
        }
    }

    public void SetMovingStatus(bool s)
    {
        isWanderMoving = s;
    }

    public bool GetMovingStatus()
    {
        return isWanderMoving;
    }

    public void SetWaitingStatus(bool s)
    {
        isWaiting = s;
    }

    public bool GetWaitingStatus()
    {
        return isWaiting;
    }

    public void SetAnimator(float f)
    {
        charAnimator.SetFloat("MovingSpeed", f);
    }

    public void SetWaiting()
    {
        isWaiting = true;
        waitIntervalNow = waitAtStation;
        lineVelocityNow = 0.0f;
        if(isGettingBack == false)
        {
            lineVelocityNext = 0.0f;
        }
    }

    public bool GetBackStatus()
    {
        return isGettingBack;
    }

    public void SetBackDirection(float s)
    {
        isGettingBack = true;
        waitIntervalNow = 3.5f;
        lineVelocityNow = 0.1f;
        lineVelocityNext = s;
        angularVelocity = 360.0f;
        targetDirection = -transform.forward;
    }

    public void SetForwardDirection(float s)
    {
        isGettingBack = true;
        waitIntervalNow = 1.75f;
        lineVelocityNext = s;
        angularVelocity = 0;
        targetDirection = transform.forward;
    }

    public void SetGivenDirection(float s, Vector3 dir)
    {
        isGettingBack = true;
        waitIntervalNow = 2.5f;
        lineVelocityNext = s;
        targetDirection = dir;
    }

    public void OnDestroyStation()
    {
        isWaiting = false;
        //this.transform.Find("MSC_Ring").gameObject.SetActive(false);
    }
}
