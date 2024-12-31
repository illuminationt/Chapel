using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpInstanceId
{
    public static FCpInstanceId Create()
    {
        FCpInstanceId newId = new FCpInstanceId();
        if (LatestId > UInt64.MaxValue - 5)
        {
            LatestId = 1;
        }
        newId._id = LatestId++;
        return newId;
    }

    public bool Equals(in FCpInstanceId other)
    {
        if (!IsValid() || !other.IsValid())
        {
            return false;
        }
        return _id == other._id;
    }

    public void Reset()
    {
        this = default;
    }
    bool IsValid()
    {
        if (_id != 0)
        {
            return true;
        }
        else { return false; }
    }
    static UInt64 LatestId = 1;
    UInt64 _id;
}
