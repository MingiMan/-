using UnityEngine;

public abstract class Props : MonoBehaviour
{
    protected DialogueManager theDM;
    protected AudioManager theAudio;
    protected FadeManager theFade;
    public Dialogue[] dialogues;
    public TextDialogue[] textDialogue;
    protected Animator animor;

    protected virtual void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theAudio = FindObjectOfType<AudioManager>();
        theFade = FindObjectOfType<FadeManager>();

        if (animor != null)
            animor = gameObject.GetComponent<Animator>();
    }

    public abstract void ShowText();
}
