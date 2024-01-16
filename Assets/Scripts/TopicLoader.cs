using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopicLoader : MonoBehaviour
{
    public GameObject topicButtonPrefab;

    private TranslatedContents translatedContents;
    private string languageID;


    private void Start()
    {
        CreateTopicButtons();
    }

    private void CreateTopicButtons()
    {
        translatedContents = Persistence.Instance.translatedContents;
        languageID = Persistence.Instance.Read("languageID.txt");

        float buttonHeight = 225f;
        float spacing = 75f;
        float nextYPos = 0;

        VerticalLayoutGroup layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childControlHeight = true;
        layoutGroup.spacing = spacing;

        ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        RectTransform csfTransform = contentSizeFitter.GetComponent<RectTransform>();
        csfTransform.anchorMin = new Vector2(0.075f, 1f);
        csfTransform.anchorMax = new Vector2(0.925f, 0.8f);

        int ID = 0;
        foreach (Topic topic in translatedContents.Contents.Topics)
        {
            GameObject button = Instantiate(topicButtonPrefab, transform);
            button.name = "Button" + topic.Name.GetText(languageID);

            int topicID = ID;
            Button buttonComponent = button.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(() => OnButtonClick(topicID));
            }

            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = new Vector2(0, nextYPos);

            nextYPos -= (buttonHeight + spacing);

            TextMeshProUGUI[] textFields = button.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI textField in textFields)
            {
                textField.font = Resources.Load<TMP_FontAsset>("Epilogue-Black.ttf");

                if (textField.gameObject.name == "Name")
                {
                    textField.text = topic.Name.GetText(languageID);
                }
                if (textField.gameObject.name == "ID")
                {
                    textField.text = (++ID).ToString();
                }
            }
        }
    }

    private void OnButtonClick(int ID) 
    {
        Persistence.Instance.Write("topicID.txt", ID.ToString());
        Persistence.Instance.GoToScene(2);
    }
}
