using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioClip boltAttack, backgroundmusic, wizardshortAttack, soldierSwiningSword, gameover;
    [SerializeField] AudioClip gameoverclip;
    static AudioSource audioSrc;
    static AudioSource audioSrcplayer;
    private void Awake()
    {

    }
    void Start()
    {
        boltAttack = Resources.Load<AudioClip>("Magic Spell_Electricity Spell_1");
        wizardshortAttack = Resources.Load<AudioClip>("Magic Spell_Simple Swoosh_6");
        soldierSwiningSword = Resources.Load<AudioClip>("Warrior_SwingingSword_V1");
        backgroundmusic = Resources.Load<AudioClip>("Cavern Atmosphere - Loop");
        //gameover = gameoverclip;
        gameover = Resources.Load<AudioClip>("game-over-sound-effect");
        audioSrc = GetComponent<AudioSource>();
        audioSrc.Play();
        // audioSrcplayer = GameObject.FindGameObjectWithTag("warrior").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null) { audioSrc.Stop(); /*audioSrc.clip = gameover; audioSrc.Play();*//*PlaySound("Magic Spell_Electricity Spell_1");*/ }
    }
    public static void PlaySound(string clip)
    {
        if (audioSrcplayer == null) { audioSrcplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>(); }
        switch (clip)
        {
            case "Magic Spell_Electricity Spell_1":
                audioSrcplayer.PlayOneShot(boltAttack);

                break;
            case "Magic Spell_Simple Swoosh_6":
                audioSrcplayer.PlayOneShot(wizardshortAttack);

                break;
            case "Warrior_SwingingSword_V1":
                audioSrcplayer.PlayOneShot(soldierSwiningSword);

                break;
            case "game-over-sound-effect":
                audioSrc.PlayOneShot(gameover);

                break;
        }

    }
    public static void StopSound(string clip)
    {
        switch (clip)
        {
            case "Magic Spell_Electricity Spell_1":



                break;
        }

    }
}
