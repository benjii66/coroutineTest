using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CS_SightData
{
    Transform transformOrigin = null;
    int angle = 0;
    float maxLenght = 0, currentLenght = 0;
    bool isObstacle = false;
    public ITarget Target { get; private set; } = null;
    public bool TargetDetected { get; private set; } = false;

    public bool IsValid => transformOrigin;

    public Vector3 SightPoint => GetSightDirection() * GetLenght();

    public CS_SightData(int _angle, Transform _transformOrigin, float _maxLenght)
    {
        transformOrigin = _transformOrigin;
        angle = _angle;
        maxLenght = _maxLenght;
    }

    public void Detection(LayerMask _targetMask, LayerMask _obstacleMask)
    {
        if (!IsValid) return;
        isObstacle = Physics.Raycast(transformOrigin.position, GetSightDirection(), out RaycastHit _hitObstacle, maxLenght, _obstacleMask);
         if (isObstacle)
            currentLenght = _hitObstacle.distance;
        bool _isTarget = Physics.Raycast(transformOrigin.position, GetSightDirection(), out RaycastHit _hitTarget, GetLenght(), _targetMask);
        ITarget _target = _isTarget ? _hitTarget.collider.GetComponent<ITarget>() : null;
        TargetDetected = _target != null;
        Target = _target;

    }

    Vector3 GetSightDirection()
    {
        if (!IsValid) return Vector3.zero;
        return Quaternion.AngleAxis(angle, transformOrigin.up) * transformOrigin.forward;
    }

    float GetLenght() => isObstacle ? currentLenght : maxLenght;

    public void OnDrawSight(bool _show)
    {
        if (!IsValid || !_show) return;
        Debug.DrawRay(transformOrigin.position, GetSightDirection() * GetLenght(), Color.Lerp(Color.red, Color.green, GetLenght() / maxLenght));
    }
}
