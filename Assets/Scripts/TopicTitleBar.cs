using UnityEngine;
using TMPro;

public class TopicTitleBar : MonoBehaviour
{
    private TranslatedContents translatedContents;
    private string LanguageID;
    private int TopicID;


    private void Start()
    {
        LoadData();
        SetTitleAndIndex();
    }

    private void LoadData()
    {
        translatedContents = Persistence.Instance.translatedContents;
        LanguageID = Persistence.Instance.Read("languageID.txt");
        TopicID = int.Parse(Persistence.Instance.Read("topicID.txt"));
    }

    private void SetTitleAndIndex()
    {
        TextMeshProUGUI[] textFields = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        foreach(TextMeshProUGUI text in textFields)
        {
            if (text.name == "ID")
            {
                text.text = (TopicID + 1).ToString();
            }
            if (text.name == "Name")
            {
                text.text = translatedContents.Contents.Topics[TopicID].Name.GetText(LanguageID);
            }
        }
    }
}
