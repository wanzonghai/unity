using System;

public class BaseManager<T> where T : new()
{
    private static T instance;
    public static T Getinstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
    public virtual void ResourcesLoading<T>(T t, string path, bool IsAsync) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public virtual void ResourcesUnLoading<T>(T t) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }
}