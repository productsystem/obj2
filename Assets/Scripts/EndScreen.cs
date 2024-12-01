using UnityEngine;

public class EndScreen : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
        }
    }
}
