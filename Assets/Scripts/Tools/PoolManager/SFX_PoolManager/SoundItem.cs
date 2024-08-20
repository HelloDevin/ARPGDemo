using UnityEngine;
using ZZZ;

public enum SoundStyle
{
    Null,
    Foot,
    Hit,
    Parry,
    ComboVoice,
    WeaponSound,
    SwitchInWindSound,
    DodgeSound,
    SwitchInVoice,
    FootBack,
    WeaponBack,
    WeaponEnd,
    SwitchTime,
}

public class SoundItem : PoolItemBase
{
    [SerializeField] private SoundStyle soundStyle;
    [SerializeField] private SoundData soundData;
    [SerializeField] private CharacterNameList CharacterNameList = CharacterNameList.Null;
    private AudioSource audioSource;
    private AudioClip clip;

    public void GetSoundData(SoundData soundData)
    {
        this.soundData = soundData;
    }

    public void SetCharacterName(CharacterNameList characterNameList)
    {
        CharacterNameList = characterNameList;
    }

    public void SetSoundStyle(SoundStyle soundStyle)
    {
        this.soundStyle = soundStyle;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Spawn()
    {
        base.Spawn();
        ReadyPlay();
    }

    private void ReadyPlay()
    {
        clip = soundData.GetAudioClip(soundStyle, CharacterNameList);
        ToPlay();
    }

    private void ToPlay()
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            StopPlay();
        }
    }

    private void StopPlay()
    {
        gameObject.SetActive(false);
    }
}