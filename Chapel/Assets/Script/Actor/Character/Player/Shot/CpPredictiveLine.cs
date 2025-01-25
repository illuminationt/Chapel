using UnityEngine;

public class CpPredictiveLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer _lineRenderer = null;

    public static CpPredictiveLine Create(CpPredictiveLine prefab, CpPlayer owner)
    {
        CpPredictiveLine instance = MonoBehaviour.Instantiate(prefab);
        instance.Initialize(owner);
        return instance;
    }

    void Initialize(CpPlayer owner)
    {
        transform.SetParent(owner.transform);
        transform.localPosition = Vector3.zero;
    }
    public void execute(float directionDeg)
    {
        Vector2 direcion = SltMath.ToVector(directionDeg);
        SetDirection(direcion);
    }

    void SetDirection(in Vector2 lineDirection)
    {
        Vector2 rootPosition = transform.position;
        // è\ï™âìÇ≠Ç»íl
        Vector2 endPosition = rootPosition + lineDirection * 500f;
        _lineRenderer.SetPosition(0, rootPosition);
        _lineRenderer.SetPosition(1, endPosition);
    }
}
