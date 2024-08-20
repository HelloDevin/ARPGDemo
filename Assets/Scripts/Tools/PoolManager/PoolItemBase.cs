using UnityEngine;

public class PoolItemBase : MonoBehaviour
{
    private void OnEnable()
    {
        Spawn();
    }
    private void OnDisable()
    {
        Recycle();
    }
    protected virtual void Spawn()
    {

    }
    protected virtual void Recycle()
    { 
    
    }

}
