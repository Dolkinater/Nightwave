using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float levelTimer;
    public float GetLevelTimer(){return levelTimer;}
    public bool isCounting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        LevelManager.OnLevelEndEvent += StoreTime;
    }
    
    void Start()
    {
        levelTimer = 0;
        isCounting = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isCounting)
        {
            levelTimer += Time.deltaTime;
        }
    }

    void StoreTime(bool competedLevel)
    {
        isCounting = false;
        
    }
}
