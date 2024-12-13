using UnityEngine;

public class AppStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SQLitePCLRaw.Startup.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
