using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IA_Detection : MonoBehaviour
{
    public event Action<ITarget> OnTargetDetected = null;
    public event Action<ITarget> OnTargetLost = null;

    [SerializeField] Transform targetTransform = null;
    [SerializeField, Range(1, 10)] int detectionRange = 8;

    bool hasDetected = false;
    ITarget target = null;

    public bool IsDetected { get; private set; } = false;
    public bool IsValid => targetTransform && target != null;
    public bool IsAtRange
    {
        get
        {
            if (!IsValid) return false;
            bool _detection = Vector3.Distance(transform.position, target.TargetPosition) < detectionRange;
            return _detection;
        }
    }

    private void Awake()
    {
        OnTargetDetected += (target) => IsDetected = true;
        OnTargetLost += (target) => IsDetected = false;
    }

    private void Start() => target = targetTransform.GetComponent<ITarget>();

    private void OnDestroy()
    {
        OnTargetDetected = null;
        OnTargetLost = null;
    }

    public void UpdateDetection()
    {
        if (!IsValid) return;
        if(IsAtRange)
        {
            OnTargetDetected?.Invoke(target);
            hasDetected = true;
        }
        else if(!IsAtRange && hasDetected)
        {
            OnTargetLost?.Invoke(target);
            hasDetected = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = !IsDetected ? Color.white : Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
