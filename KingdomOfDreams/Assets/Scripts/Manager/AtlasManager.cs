using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager : MonoBehaviour
{
    public static AtlasManager instance;

    public SpriteAtlas[] arrAtlas;
    public Dictionary<string, SpriteAtlas> dicAtlas = new Dictionary<string, SpriteAtlas>();

    private void Awake()
    {
        AtlasManager.instance = this;

        for (int i = 0; i < this.arrAtlas.Length; i++)
        {
            var atlas = this.arrAtlas[i];

            var atlasName = atlas.name.Replace("Atlas", "");
            this.dicAtlas.Add(atlasName, atlas);
        }
    }

    public SpriteAtlas GetAtlasByName(string name)
    {
        if (this.dicAtlas.ContainsKey(name))
        {
            return this.dicAtlas[name];
        }
        else
        {
            Debug.LogFormat("<color=red>key: {0}을 못찾음.</color>", name);
            return null;
        }
    }
}
