using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using System;
using NUnit.Framework;

public interface ICpContainable
{
    public static void OnAwakeContainable<T>(ICpContainable self) where T : ICpContainable
    {
#if CP_DEBUG
        if (!IsValidInterface<T>())
        {
            return;
        }
#endif
        CpInterfaceContainer.Get().Register<T>(self);
    }
    public static void OnDestroyContainable<T>(ICpContainable self) where T : ICpContainable
    {
#if CP_DEBUG
        if (!IsValidInterface<T>())
        {
            return;
        }
#endif

        CpInterfaceContainer.Get().Unregister<T>(self);
    }


    static bool IsValidInterface<T>() where T : ICpContainable
    {
        if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
        {
            Assert.IsTrue(false, $"{typeof(T).Name}はMonoBehaviorを継承しています。MonoBehavior継承クラスではなく、ICpContainable継承クラスを指定してください。");
            return false;
        }
        return true;
    }
}

public class CpInterfaceContainer
{
    public static CpInterfaceContainer Create()
    {
        CpInterfaceContainer instance = new CpInterfaceContainer();
        return instance;
    }
    public static CpInterfaceContainer Get()
    {
        return CpGameManager.Instance.InterfaceContainer;
    }

    public void Register<T>(ICpContainable containableInterface)
    {
        System.Type type = typeof(T);
        if (_dict.ContainsKey(type))
        {
            List<ICpContainable> list = _dict[type];
            if (!list.Contains(containableInterface))
            {
                list.Add(containableInterface);
            }
        }
        else
        {
            List<ICpContainable> newList = new List<ICpContainable>();
            newList.Add(containableInterface);
            _dict.Add(type, newList);
        }
    }
    public void Unregister<T>(ICpContainable containableInterface)
    {
        var list = Find<T>();
        Assert.IsTrue(list != null);
        Assert.IsTrue(list.Contains(containableInterface));
        list.Remove(containableInterface);
    }

    public bool ForEach<T>(Func<T, bool> func) where T : class, ICpContainable
    {
        List<ICpContainable> list = Find<T>();
        if (list == null)
        {
            return false;
        }

        foreach (ICpContainable containable in list)
        {
            T t = containable as T;
            Assert.IsTrue(t != null);
            if (func(t))
            {
                return true;
            }
        }
        return false;
    }
    public bool ForEach<T>(Action<T> action) where T : class, ICpContainable
    {
        Func<T, bool> func = (T t) => { action(t); return false; };
        return ForEach<T>(func);
    }

    List<ICpContainable> Find<T>()
    {
        System.Type type = typeof(T);
        if (_dict.ContainsKey(type))
        {
            List<ICpContainable> list = _dict[type];
            return list;
        }
        return null;
    }

    Dictionary<System.Type, List<ICpContainable>> _dict = new Dictionary<System.Type, List<ICpContainable>>();

#if CP_DEBUG
    public void DebugUpdate()
    {
        // バリデーション処理

    }
#endif
}
