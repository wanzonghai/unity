using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TextureMgr : BaseManager<TextureMgr>
{

    //图集的集合，Dictionary<[key], [value]>，方便处理 
    public Dictionary<string, Object[]> m_pAtlasDic;

    public void init()
    {
        //初始化字典
        if (m_pAtlasDic == null)
        {
            m_pAtlasDic = new Dictionary<string, Object[]>();
        }
    }
    //删除图集缓存  
    public void DeleteAtlas(string _spriteAtlasPath)
    {  //字典的键为路径
        if (m_pAtlasDic.ContainsKey(_spriteAtlasPath))
        {  //ContainsKey为Dictionary的方法，判断是否存在指定键名，不可以使用get来判断是否存在某个键，因为当键存在，值不存在，也返回null
            m_pAtlasDic.Remove(_spriteAtlasPath);  //Remove为Dictionary方法，移除键值
        }
    }
    /// <summary>
    /// 加载Atlas资源
    /// </summary>
    public void LoadResAtlas(string _spriteAtlasPath)
    {
        Object[] _atlas = Resources.LoadAll(_spriteAtlasPath);

        if (!m_pAtlasDic.ContainsKey(_spriteAtlasPath)) { m_pAtlasDic.Add(_spriteAtlasPath, _atlas); }
    }
    /// <summary>
    /// 纹理图集加载精灵
    /// _spriteAtlasPath 图集路径
    /// _spriteName 精灵图名
    /// </summary>
    public Sprite LoadResAtlasSprite(string _spriteAtlasPath, string _spriteName)
    {

        Sprite _sprite = FindSpriteFormBuffer(_spriteAtlasPath, _spriteName);
       
        //当_sprite找不到，说明m_pAtlasDic的Object[]为空，还未缓存Object[]
        if (_sprite == null) { 
            Object[] _atlas = Resources.LoadAll(_spriteAtlasPath);

            if(!m_pAtlasDic.ContainsKey(_spriteAtlasPath)) {m_pAtlasDic.Add (_spriteAtlasPath,_atlas); } 
            _sprite = SpriteFormAtlas (_atlas,_spriteName);  //SpriteFormAtlas，直接从图集本身查找
        }  
        return _sprite; 

    }
    /// <summary>
    /// 从缓存中查找图集，并找出sprite
    /// </summary>
    /// <param name="_spriteAtlasPath">图集路径，</param>
    /// <param name="_spriteName">精灵图名</param>
    private Sprite FindSpriteFormBuffer(string _spriteAtlasPath, string _spriteName)
    
        {
        if (m_pAtlasDic.ContainsKey(_spriteAtlasPath))
        {  //如果键名存在
            Object[] _atlas = m_pAtlasDic[_spriteAtlasPath]; //把m_pAtlasDic值保存起来，即图集数组 
            Sprite _sprite = SpriteFormAtlas(_atlas, _spriteName);  //在m_pAtlasDic值中寻找
            return _sprite;
        }
        return null;
    }
    /// <summary>
    /// 从图集中，并找出sprite  
    /// </summary>
    /// <param name="_atlas">图集数组</param>
    /// <param name="_spriteName">精灵图名</param>
    /// <returns></returns>
    private Sprite SpriteFormAtlas(Object[] _atlas, string _spriteName)
    { 
        for (int i = 0; i < _atlas.Length; i++)
        {
            /*if (_atlas[i].GetType() == typeof(UnityEngine.Sprite))
            {
               
            }*/
            if (_atlas[i].name == _spriteName)
            {
                return (Sprite)_atlas[i];
            }
        }
        Debug.LogWarning("sprite:" + _spriteName + ";undefinde in atlas");  //如果没有return
        return null;
    }
}
