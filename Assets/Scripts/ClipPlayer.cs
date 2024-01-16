using UnityEngine;

public class ClipPlayer : AudioPlayerController
{
    private TranslatedContents translatedContents;
    private string LanguageID;
    private int TopicID;


    private void Start()
    {
        LoadData();
        PlayClip();
    }

    private void Update()
    {
        UpdateProgressBar();
    }

    private void LoadData()
    {
        translatedContents = Persistence.Instance.translatedContents;
        LanguageID = Persistence.Instance.Read("languageID.txt");
        TopicID = int.Parse(Persistence.Instance.Read("topicID.txt"));
    }

    private void PlayClip()
    {
        Persistence.Instance.GetAudioClip(TopicID, (AudioClip clip) =>
        {
            audioSource.clip = clip;
            audioSource.Play();
        });
    }
}