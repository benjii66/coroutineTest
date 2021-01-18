using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_WayPointsSystem : MonoBehaviour
{
    #region F/P
    [SerializeField] List<Vector3> waypoints = new List<Vector3>();
    [SerializeField] int indexPoint = -1;
    [SerializeField, Header("Color WayPoint")] Color wayPointColor = Color.green;
    [SerializeField, Header("Color WayPoints Link")] Color wayPointsLinkColor = Color.blue;
    #endregion F/P

    #region UnityMethods
    void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.color = wayPointColor;
            Gizmos.DrawWireSphere(waypoints[i], .5f);
            if (i < waypoints.Count - 1)
            {
                Gizmos.color = wayPointsLinkColor;
                Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
            }

        }
        if (indexPoint < 0) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(waypoints[indexPoint], Vector3.one);
    }
    #endregion UnityMethods

    #region CustomMethods
    public Vector3 PickPoint()
    {
        indexPoint++;
        indexPoint %= waypoints.Count;
        return waypoints[indexPoint];
    }
    public void AddPoint()
    {
        Vector3 _point = waypoints.Count == 0 ? Vector3.zero : waypoints[waypoints.Count - 1] + Vector3.forward;
        waypoints.Add(_point);
    }
    public void Clear() => waypoints.Clear();
    #endregion CustomMethods

}
