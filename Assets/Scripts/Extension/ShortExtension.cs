using UnityEngine;

public static class ShortExtension
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T:Component
    {
        T comp = obj.GetComponent<T>();
        if(comp == null)
        {
            comp = obj.AddComponent<T>();
        }
        return comp;
    }
}
