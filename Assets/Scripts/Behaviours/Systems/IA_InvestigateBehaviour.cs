using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_InvestigateBehaviour : MonoBehaviour
{
    [SerializeField, Range(1, 20)] float investigationRadius = 5;
    Vector3 investigationStart = Vector3.zero, investigationPoint = Vector3.zero;
    float baseRadius = 0;

    List<Vector3> historicPoints = new List<Vector3>();


    void Awake() => baseRadius = investigationRadius;

    public void SetRadiusMultiplicator(float _multiplier)
    {
        if (_multiplier == 0) return;
        investigationRadius = baseRadius * _multiplier;
    }

    public void SetLastSeenTarget(Vector3 _lastPoint)
    {
        investigationRadius = baseRadius;
        historicPoints.Clear();
        investigationStart = _lastPoint;
    }

    public Vector3 GetInvestigationPoint()
    {
        int _angle = Random.Range(0, 360);
        float _x = Mathf.Cos(_angle * Mathf.Deg2Rad) * investigationRadius;
        float _y = 0;
        float _z = Mathf.Sin(_angle * Mathf.Deg2Rad) * investigationRadius;
        investigationPoint = new Vector3(_x, _y, _z) + investigationStart;
        historicPoints.Add(investigationPoint);
        return investigationPoint;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(investigationStart, Vector3.one);
        Gizmos.DrawLine(transform.position, investigationStart);
        Gizmos.color = Color.cyan - new Color(0, 0, 0, .8f);
        Gizmos.DrawSphere(investigationStart, investigationRadius);
        for (int i = 0; i < historicPoints.Count; i++)
        {
            Gizmos.color = Color.Lerp(Color.red, Color.green, (float)i / historicPoints.Count);
            Gizmos.DrawWireCube(historicPoints[i], Vector3.one * .5f);
            Gizmos.DrawLine(historicPoints[i], investigationStart);
        }
        Gizmos.DrawLine(transform.position, investigationPoint);
    }
}
