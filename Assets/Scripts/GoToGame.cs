using UnityEngine;
using UnityEngine.UI;

public class GoToGame : MonoBehaviour
{
    private void Start() 
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Persistence.Instance.GoToScene(3));     
    }
}
