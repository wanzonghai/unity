using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesLoadingMode :  IResourcesLoadingMode
{
    public Image Img;
    public MonoBehaviour MB;

    public virtual void ResourcesLoading<T>(T t, string path, bool IsAsync) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public virtual void ResourcesUnLoading<T>(T t) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public virtual void AssetDatabaseLoading<T>(T t, string path, bool IsAsync) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public virtual void AssetDatabaseUnLoading<T>(T t) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public virtual void WWWLoading<T>(T t, string path, bool IsAsync) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

    public virtual void WWWUnLoading<T>(T t) where T : UnityEngine.Object
    {
        throw new NotImplementedException();
    }

}