using UnityEngine;

public class UIDisplayer : MonoBehaviour
{

    private void Start()
    {
        UIDisplayManager.OnDisplayingStateChanged += HandleDisplayStateChanged;
    }

    private void OnDestroy()
    {

        UIDisplayManager.OnDisplayingStateChanged -= HandleDisplayStateChanged;
    }

    private void HandleDisplayStateChanged(bool isDisplayed)
    {
        this.gameObject.SetActive(isDisplayed);  //enabled = isDisplayed;
        //_canvas.enabled = isDisplayed;
    }
}
