using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ResManage: ResourcesLoadingMode
{
    public override void ResourcesLoading<T>(T t, string path, bool IsAsync)
    {
     
        if (IsAsync == false)
        {
            T load = Resources.Load<T>(path);
            t = load;

            Debug.Log("====" + path + "====");
            if (t.GetType() == Img.sprite.GetType())
            {
                Img.sprite = t as Sprite;
                Resources.UnloadAsset(t);
            }
        }
        else
        {
            T load = Resources.LoadAsync<T>(path).asset as T;
            t = load;
            if (t.GetType() == Img.sprite.GetType())
            {
                Img.sprite = t as Sprite;
                Resources.UnloadAsset(t);
            }
        }
    }
#if UNITY_EDITOR
    public override void AssetDatabaseLoading<T>(T t, string path, bool IsAsync)
    {
        if (IsAsync == false)
        {
            //s=string.Format( "Assets/Image/{0}.jpg",Index.ToString())
            T load = AssetDatabase.LoadAssetAtPath<T>(path);
            Debug.Log(path);
            t = load;
            Debug.Log(t.name);
            if (t.GetType() == Img.sprite.GetType())
            {
                Img.sprite = t as Sprite;
                Resources.UnloadAsset(t);
            }
            else
            {
                Debug.Log(t.name);
            }
        }

        else
        {
            Debug.Log("assetdatabase没有异步加载");
        }
    }
#endif
    public override void WWWLoading<T>(T t, string path, bool isAsync)
    {
        if (isAsync == true)
        {
            // MB.StartCoroutine(WWWLoad());
        }
        else
        {
            Debug.Log("----WWW没有同步加载----");
        }
    }

    public override void WWWUnLoading<T>(T t)
    {
        base.ResourcesUnLoading<T>(t);

    }

    [Obsolete]
    public static IEnumerator WWWLoad(string url, Action<WWW> callback)
    {
        Debug.Log("----WWW协程调用----");
        //string url = string.Format(@"file://{0}/{1}.jpg", Application.streamingAssetsPath, Index.ToString());
        WWW www = new WWW(url);
        yield return www;
        callback.Invoke(www);
        yield return null;
    }
    /// <summary>
    /// WWW回调函数
    /// </summary>
    /// <param name="obj"></param>
    /*private void WWWcallback(object obj)
    {
        WWW www = obj as WWW;
        Texture2D tex = www.texture;
        BG.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        Debug.Log("----callback调用----");
        www.Dispose();
    }*/
    ///UnityWebRequest方式：
    /*public override void ResourcesLoading<T>(T t, string path, bool isAsync)
    {

        if (isAsync == false)
        {
            Debug.Log("unityWebRequest没有同步加载");
        }
    }

    public override void ResourcesUnLoading<T>(T t)
    {
        base.ResourcesUnLoading<T>(t);
    }

    public static IEnumerator UnityWebRequestLoad(string url, Action<UnityWebRequest> callback)
    {
        //string url =string.Format( @"file://{0}/{1}.jpg", Application.streamingAssetsPath,Index.ToString());
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            callback.Invoke(webRequest);
            yield return null;
            //Texture2D tex = DownloadHandlerTexture.GetContent(webRequest);
            //Img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
    }*/
    /// <summary>
    /// UnityWebRequest回调函数
    /// </summary>
    /// <param name="obj"></param>
   /* private void unityWebRequestcallback(object obj)
    {
        UnityWebRequest request = obj as UnityWebRequest;
        Texture2D tex = DownloadHandlerTexture.GetContent(request);
        BG.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        request.Dispose();
    }*/


}
