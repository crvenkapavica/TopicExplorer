using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageButtons : MonoBehaviour
{
    public GameObject languageButtonPrefab;

    private TranslatedContents translatedContents;


    private void Start()
    {
        CreateLanguageButtons();
    }

    private void CreateLanguageButtons()
    {
        translatedContents = Persistence.Instance.translatedContents;

        GameObject buttonsContainer = new GameObject("ButtonsContainer");
        RectTransform ButtonsContainer = buttonsContainer.AddComponent<RectTransform>();
        ButtonsContainer.SetParent(transform, false);
        ButtonsContainer.anchorMin = new Vector2(0.1f, 0.45f);
        ButtonsContainer.anchorMax = new Vector2(0.9f, 1f);

        float buttonHeight = 150f;
        float spacing = 30f;
        float nextYPos = 0f;

        // Hard limit na 3 jezika na main screenu
        // Moze se staviti i scroll bar ali posto nista nije navedeno je limitirano na 3 jezika
        for (int i = 0; i < translatedContents.languages.Count && i < 3; i++)
        {
            GameObject button = Instantiate(languageButtonPrefab, ButtonsContainer);
            button.name = "Button" + translatedContents.languages[i].name;

            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = new Vector2(0, nextYPos);
            nextYPos -= (buttonHeight + spacing);

            int index = i;
            Button buttonComponent = button.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => OnLanguageButtonClick(translatedContents.languages[index].id));
            }

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>(true);
            if (buttonText != null)
            {
                buttonText.text = translatedContents.languages[i].name;
            } 
        }
    }

    private void OnLanguageButtonClick(string languageID)
    {
        Persistence.Instance.Write("languageID.txt", languageID);
        Persistence.Instance.GoToScene(1);
    }
}
