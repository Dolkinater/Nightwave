using UnityEngine;

public class DestroyBarrier : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToCheck;

    float timer;

    void Update()
    {
        if (timer <= 0)
        {
            timer = .3f;

            RunCheck();
        }

        timer -= Time.deltaTime;
    }

    void RunCheck()
    {
        for (int i = 0; i < objectsToCheck.Length; i++)
        {
            if (objectsToCheck[i] != null) { return; }
        }

        Destroy(gameObject);
    }
}
