using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isPaused;
    [SerializeField] GameObject dieScreen;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject player;
    [SerializeField] GameObject settingCanvas;
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject playerPos;
    static int index = 0;
    AudioSource audioSrc;
    private static float playedgames;

    // Start is called before the first frame update
    void Awake()
    {

        /*  if (instance == null)
          {
              instance = this;
          }
          else { Destroy(gameObject); }
          DontDestroyOnLoad(gameObject);*/
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu" || SceneManager.GetActiveScene().name != "Selection menu") { InstantiatePlayer(); }

        isPaused = false;
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Health health = player.GetComponent<Health>();
                if (health != null)
                {
                    health.dieEvent.AddListener(OnDieListener);
                }
                else
                {
                    Debug.Log("GM: health not found");
                }
            }
            else
            {
                Debug.Log("GM: player GO not found");
            }
        }
        // if (dieScreen != null)
        // {
        //     if (Health.died)
        //     {
        //         audioSrc.Play();
        //         Destroy(player);
        //         Invoke("DieSCreen", 0.5f);
        //     }
        // }
        if (Input.GetKeyDown(KeyCode.Escape)) { if (!isPaused) { Pause(); } else { Resume(); } }

    }

    private void OnDieListener()
    {
        Debug.Log("GM: " + player + " : dead ! ");
        audioSrc.Play();

        GameObject spawner = GameObject.FindGameObjectWithTag("Spawner");
        if (spawner == null)
        {
            Debug.LogError("Spawner not found");
        }
        else
        {
            string character = player.GetComponent<Character>().GetName();
            Spawning spawning = spawner.GetComponent<Spawning>();
            if (spawning == null)
            {
                Debug.LogError("Spawning not found");
            }
            else
            {
                int wave = spawning.nextWave + 1;
                if (wave > Character.GetMaxWaves(character))
                {
                    Character.SaveStat(character, Character.MAX_WAVES, wave);
                }
            }
        }

        Destroy(player);

        if (dieScreen != null)
        {
            Invoke("DieSCreen", 0.5f);
            Invoke("MainMenu", 2);
        }
    }

    void DieSCreen() { dieScreen.SetActive(true); inventory.SetActive(false); }
    public void Pause() { isPaused = true; Time.timeScale = 0; settingCanvas.SetActive(true); }
    public void Resume() { isPaused = false; Time.timeScale = 1; settingCanvas.SetActive(false); }

    public void MainMenu() { SceneManager.LoadScene("Main Menu", LoadSceneMode.Single); }
    public void carachter(int i) { index = i; SceneManager.LoadScene("Arena1", LoadSceneMode.Single); }
    public void Quit() { Application.Quit(); }
    public void InstantiatePlayer() { Instantiate(players[index], playerPos.transform.position, Quaternion.identity); print("player instatiated " + index); }
    public void Level(string levelname)
    {
        SceneManager.LoadScene(levelname, LoadSceneMode.Single); if (levelname == "Arena1") { string character = player.GetComponent<Character>().GetName(); Character.SaveStat(character, Character.PLAYED_GAMES, playedgames++); }
    }
}
