using UnityEngine;

public enum ECpLocatorType
{
    None = -1,
    Index = 0,
    String = 100,
}

public enum ECpLocatorIndex
{
    None = -1,
    A = 0,
    B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y,
    Z = 25,
}

[System.Serializable]
public class CpLocatorParam
{
    public bool IsValidParam()
    {
        return _locatorType switch
        {
            ECpLocatorType.Index => IsValidParamIndex(),
            ECpLocatorType.String => IsValidParamString(),
            _ => false
        };
    }
    bool IsValidParamIndex()
    {
        if (_locatorType == ECpLocatorType.None)
        {
            return false;
        }
        return true;
    }
    bool IsValidParamString()
    {
        return _string != null;
    }

    public bool IsMatch(CpLocatorParam otherParam)
    {
        if (!IsValidParam() || !otherParam.IsValidParam())
        {
            return false;
        }

        if (_locatorType != otherParam._locatorType)
        {
            return false;
        }

        return _locatorType switch
        {
            ECpLocatorType.Index => IsMatchIndex(otherParam),
            ECpLocatorType.String => IsMatchString(otherParam),
            _ => throw new System.NotImplementedException(),
        };
    }

    bool IsMatchIndex(CpLocatorParam other)
    {
        return _Index == other._Index;
    }

    bool IsMatchString(CpLocatorParam other)
    {
        return _string.Equals(other._string);
    }

    [SerializeField]
    ECpLocatorType _locatorType = ECpLocatorType.None;

    [SerializeField]
    ECpLocatorIndex _Index = ECpLocatorIndex.None;

    [SerializeField]
    string _string = null;
}

public class CpLocator : MonoBehaviour
{
    [SerializeField]
    CpLocatorParam _locatorParam = null;

    public Vector2 GetLocation()
    {
        return transform.position;
    }
    public bool IsLocateConditionMatch(CpLocatorParam otherParam)
    {
        return _locatorParam.IsMatch(otherParam);
    }

    private void OnGUI()
    {

    }
}
