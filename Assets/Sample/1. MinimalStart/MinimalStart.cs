using UnityEngine;

public class MinimalStart : MonoBehaviour
{
    void Awake()
    {
        SQLitePCLRaw.Startup.Setup();
    }
}
