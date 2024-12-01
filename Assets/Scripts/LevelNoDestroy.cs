using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNoDestroy : MonoBehaviour
{
    private static LevelNoDestroy instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "EndScreen")
        {
            Destroy(gameObject);
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}