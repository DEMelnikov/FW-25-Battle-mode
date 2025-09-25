using UnityEngine;

public class SceneManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PauseManager.TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            UIDisplayManager.ToggleDisplaying();
        }
    }    
}
