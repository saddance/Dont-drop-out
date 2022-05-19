using System;
using UnityEngine;

[Serializable]
public class Vector2IntS
{
    public int x;
    public int y;

    public Vector2IntS(Vector2Int v)
    {
        x = v.x;
        y = v.y;
    }

    public Vector2Int GetV()
    {
        return new Vector2Int(x, y);
    }

    public static implicit operator Vector2IntS(Vector2Int v)
    {
        return new Vector2IntS(v);
    }
}

[Serializable]
public class Vector3S
{
    public float x;
    public float y;
    public float z;

    public Vector3S(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public Vector3 Get()
    {
        return new Vector3(x, y, z);
    }

    public static implicit operator Vector3S(Vector3 v)
    {
        return new Vector3S(v);
    }
}

[Serializable]
public class ColorS
{
    public float r;
    public float g;
    public float b;
    public float a;


    public ColorS(Color c)
    {
        r = c.r;
        g = c.g;
        b = c.b;
        a = c.a;
    }

    public Color Get()
    {
        return new Color(r, g, b, a);
    }

    public static implicit operator ColorS(Color c)
    {
        return new ColorS(c);
    }
}