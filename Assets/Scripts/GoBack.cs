using UnityEngine;
using UnityEngine.UI;

public class GoBack : MonoBehaviour
{
    private void Start()
    {
        Button backButton = GetComponent<Button>();
        backButton.onClick.AddListener(Persistence.Instance.GoBackToPreviousScene);   
    }
}