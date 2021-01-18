using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IA_Movements : MonoBehaviour
{
    #region F/P
    public event Action OnPositionReached = null;
    public event Action OnTargetSet = null;

    [SerializeField] Vector3 targetPosition = Vector3.zero;
    [SerializeField] float speed = 2.5f;
    [SerializeField, Range(.1f, 10)] float atPosRange = .1f;
    public bool IsAtPosition => Vector3.Distance(transform.position, targetPosition) < atPosRange;
    #endregion

    #region Custom Methods
    public void SetTarget(Vector3 _target)
    {
        targetPosition = _target;
        OnTargetSet?.Invoke();
    }
    public void MoveTo()
    {
        if(IsAtPosition)
        {
            OnPositionReached?.Invoke();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
        transform.LookAt(targetPosition);
    }
    #endregion

    #region Gizmos Methods
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.1f);
    }
    #endregion
}