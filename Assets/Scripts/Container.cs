using System;
using System.Collections.Generic;

public interface IContainer
{
    T GetInstance<T>();
    void SetInstance<T>(object obj);

}

public class Container : IContainer
{
    Dictionary<Type, object> instanceDic;
    public Container()
    {
        this.instanceDic = new Dictionary<Type, object>();
    }

    public T GetInstance<T>()
    {
        instanceDic.TryGetValue(typeof(T), out object obj);
        return (T)obj;

    }

    public void SetInstance<T>(object obj)
    {
        instanceDic.Add(typeof(T), obj);
    }
}
