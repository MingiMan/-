using UnityEngine;

public class AmuletEventScene : MonoBehaviour
{
    bool IsEvent;
    bool IsActive;
    [SerializeField] GameObject washingMachine;
    [SerializeField] GameObject tub;
    public string washMachin_Sound;
    AudioManager theAudio;
    PlayerManager thePlayer;

    private void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnEnable()
    {
        IsEvent = true;
    }

    private void Update()
    {
        if (thePlayer.currentMapName == "BathRoom" && IsEvent)
        {
            if (!IsActive)
            {
                WashingMachineOn();
                IsActive = true;
            }
        }
        else
        {
            if (IsActive)
            {
                WashingMachineOff();
                IsActive = false;
            }
        }
    }

    public void WashingMachineOn()
    {
        theAudio.SoundPlay(washMachin_Sound);
        washingMachine.GetComponent<Animator>().SetBool("IsWorking", true);
    }

    public void WashingMachineOff()
    {
        theAudio.SoundStop(washMachin_Sound);
        washingMachine.GetComponent<Animator>().SetBool("IsWorking", false);
    }
}
