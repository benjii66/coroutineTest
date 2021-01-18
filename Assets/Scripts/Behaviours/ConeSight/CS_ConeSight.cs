using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CS_ConeSight : MonoBehaviour
{
    public event Action<ITarget> OnTargetInSight = null;
    public event Action<Vector3> OnTargetLost = null;
    public event Action OnUpdateSightDebug = null;
    public event Action OnUpdateDelay = null;

    [SerializeField, Range(0, 360)] int sightAngle = 90;
    [SerializeField, Range(10, 100)] float sightDistance = 10;
    [SerializeField] LayerMask targetMask = 0, obstacleMask = 0;
    [SerializeField, Range(.1f, 1)] float updateTickRate = .1f;
    [SerializeField] bool showDebug = false;
    bool targetInSight = false;
    CS_SightData[] sightDatas = null;
    ITarget lastTarget = null;

    #region coneMesh
    Mesh coneMesh = null;
    [SerializeField] MeshFilter coneMeshFilter = null;
    [SerializeField] MeshRenderer coneMeshRenderer = null;
    [SerializeField] Color inactiveColor = Color.green, searchColor = Color.yellow, attackColor = Color.red;

    Color currentConeColor = Color.white;
    public bool IsValidMesh => coneMeshFilter && coneMeshRenderer;
    #endregion

    void Start()
    {
        currentConeColor = inactiveColor;
        GenerateSight();
        OnTargetInSight += (target) => currentConeColor = attackColor;
        OnTargetLost += (target) => currentConeColor = searchColor;
    }

    void Update()
    {
        OnUpdateSightDebug?.Invoke();
        UpdateConeColor();
    }

    void OnDestroy()
    {
        OnUpdateSightDebug = null;
        OnUpdateDelay = null;
    }

    public void StartConeSight()
    {
        InvokeRepeating("UpdateConeSightDelay", 0, updateTickRate);
    }
    public void StopConeSight()
    {
        CancelInvoke("UpdateConeSightDelay");
    }

    void GenerateSight()
    {
        OnUpdateSightDebug = null;
        OnUpdateDelay = null;
        int _index = 0;
        sightDatas = new CS_SightData[sightAngle];
        for (int i = -sightAngle/2; i < sightAngle/2; i++)
        {
            CS_SightData _sight = new CS_SightData(i, transform, sightDistance);
            OnUpdateSightDebug += () => _sight.OnDrawSight(showDebug);
            OnUpdateDelay += () => _sight.Detection(targetMask, obstacleMask);
            sightDatas[_index] = _sight;
            _index++;
        }
        OnUpdateDelay += () =>
        {
            DrawConeSight();
            CheckSightInfo();
        };
    }

    void UpdateConeSightDelay() => OnUpdateDelay?.Invoke();

    void CheckSightInfo()
    {
        targetInSight = sightDatas.Any(s => s.TargetDetected);
        if (!targetInSight && lastTarget != null)
        {
            OnTargetLost?.Invoke(lastTarget.TargetPosition);
            lastTarget = null;
        }
        if(targetInSight /*&& lastTarget == null*/)
        {
            CS_SightData _data = sightDatas.FirstOrDefault(s => s.TargetDetected);
            lastTarget = _data.Target;
            OnTargetInSight?.Invoke(_data.Target);
        }
    }


    #region coneMeshMethods

    void DrawConeSight()
    {
        int _vertexCount = sightAngle + 1;
        Vector3[] _vertices = new Vector3[_vertexCount];
        int[] _triangles = new int[(_vertexCount - 2) * 3];
        _vertices[0] = Vector3.zero;
        for (int i = 0; i < _vertexCount - 1; i++)
        {
            _vertices[i + 1] = transform.InverseTransformVector(sightDatas[i].SightPoint);
            if(i < _vertexCount - 2)
            {
                _triangles[i * 3] = 0;
                _triangles[i * 3 + 1] = i + 1;
                _triangles[i * 3 + 2] = i + 2;
            }
        }
        coneMesh = new Mesh();
        coneMesh.Clear();
        coneMesh.vertices = _vertices;
        coneMesh.triangles = _triangles;
        coneMesh.RecalculateNormals();
        coneMeshFilter.mesh = coneMesh;
    }

    #endregion

    void UpdateConeColor()
    {
        if (!IsValidMesh) return;
        coneMeshRenderer.material.color = Color.Lerp(coneMeshRenderer.material.color, currentConeColor, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Vector3 _offsetUp = transform.position + Vector3.up * 2;
        //Gizmos.color = targetDetect ? Color.green : Color.grey;
        Gizmos.DrawLine(transform.position, _offsetUp);
        Gizmos.DrawSphere(_offsetUp, .5f);
    }
}
