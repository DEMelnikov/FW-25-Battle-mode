using TMPro;
using UnityEngine;

public class TMPPauseStateListener : MonoBehaviour
{
    private TMP_Text TextField;
    // = this.gameObject.GetComponent<TMP_Text>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextField = this.gameObject.GetComponent<TMP_Text>();
        PauseManager.OnPauseStateChanged += HandlePauseChange;
        HandlePauseChange(PauseManager.IsPaused);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HandlePauseChange(bool isPaused)
    {
        //Debug.Log(isPaused ? "Игра на паузе" : "Игра продолжается");
        if (isPaused) TextField.SetText("||");
        else TextField.SetText(">");
        // Можно, например, включать/выключать панель паузы
    }

    void OnDestroy()
    {
        PauseManager.OnPauseStateChanged -= HandlePauseChange; // Отписываемся при уничтожении
    }
}

//void Start()
//{
//    PauseManager.OnPauseStateChanged += HandlePauseChange;
//}

//void HandlePauseChange(bool isPaused)
//{
//    Debug.Log(isPaused ? "Игра на паузе" : "Игра продолжается");
//    // Можно, например, включать/выключать панель паузы
//}

//void OnDestroy()
//{
//    PauseManager.OnPauseStateChanged -= HandlePauseChange; // Отписываемся при уничтожении
//}
