using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Gallery : MonoBehaviour
{
    private Image backgroundImage;
    private TranslatedContents translatedContents;
    private List<Sprite> images = new List<Sprite>(); 
    private string LanguageID;
    private int TopicID;

    private int currentImageIndex = 0;


    private void Start()
    {
        LoadData();
        StartCoroutine(ChangeImageRoutine());
        SetDetails();
    }

    private void LoadData()
    {
        translatedContents = Persistence.Instance.translatedContents;
        TopicID = int.Parse(Persistence.Instance.Read("topicID.txt"));
        LanguageID = Persistence.Instance.Read("languageID.txt");

        backgroundImage = GetComponent<Image>();

        foreach (Media media in translatedContents.Contents.Topics[TopicID].Media)
        {
            if (media.Name == "Gallery")
            {
                foreach (Photo photo in media.Photos)
                {
                    Sprite loadedSprite = LoadSprite(photo.Path);
                    images.Add(loadedSprite);
                }
            }
        }
    }

    private Sprite LoadSprite(string relativePath)
    {
        string path = Path.Combine(Application.persistentDataPath, relativePath);

        if (File.Exists(path))
        {
            byte[] data = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            if (texture.LoadImage(data))
            {
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
        return null;
    }

    private IEnumerator ChangeImageRoutine()
    {
        while (true)
        {
            if (images.Count > 0)
            {
                backgroundImage.sprite = images[currentImageIndex++];
                currentImageIndex %= images.Count;
            }
            yield return new WaitForSeconds(5); 
        }
    }

    private void SetDetails()
    {
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = translatedContents.Contents.Topics[TopicID].Details.GetText(LanguageID);
    }
}