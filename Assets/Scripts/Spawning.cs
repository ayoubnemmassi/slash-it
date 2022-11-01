using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawning : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    // Start is called before the first frame update
    //[SerializeField] List<GameObject> ennemyPrefabs;
    [SerializeField] Wave wave;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] public int nextWave = 0;
    [SerializeField] int bosswave;
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] GameObject waveanimation;
    [SerializeField] GameObject waveBossanimation;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject winCanvas;
    [SerializeField] TextMeshProUGUI waveanimationText;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] float waveCountDown;
    [SerializeField] string level;
    float waveswon;
    float searchCountDown = 1f;
    SpawnState state = SpawnState.COUNTING;
    public float waveDuration;
   public List<float> WavesDuration;
    [SerializeField]private Transform boss;

    void Start()
    {
        waveCountDown = timeBetweenWaves;

        /* foreach (GameObject enemy in ennemyPrefabs)
         {
             Instantiate(enemy, transform.position, Quaternion.identity);
         }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {

            if (!enemyIsAlive())

            {
                WaveCompleted();
            }
            else { waveDuration += Time.deltaTime;
                return;
            }
        }
        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {


                if (nextWave > bosswave) { winCanvas.SetActive(true); Invoke("Wining", 5); }
                else
                {
                    if (nextWave == bosswave) { waveText.text = "Wave" + nextWave.ToString(); waveBossanimation.SetActive(true); SpawnEnemy(boss); }
                    else
                    {
                        waveanimationText.text = "WAVE " + nextWave.ToString();
                        waveText.text = "Wave" + nextWave.ToString();
                        waveanimation.SetActive(true);
                        

                    }
                    StartCoroutine(SpawnWave(wave));
                }

            }
          
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }
    bool enemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            return GameObject.FindGameObjectWithTag("Enemy") == null ? false : true;
        }
        return true;
    }
    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        if (nextWave < 11)
        {
            for (int i = 0; i < fibonacci(nextWave); i++)
            {

                SpawnEnemy(_wave.enemies[Random.Range(0, 3)]);
                yield return new WaitForSeconds(1f / _wave.rate);

            }
            state = SpawnState.WAITING;
            
            yield break;
        }
        else { WaveCompleted(); }

    }
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;
        WavesDuration.Add(waveDuration);
        waveDuration = 0;

        if (nextWave + 1 > 10)
        {
            print(" congratulations waves completed");
        }
        else
        {
            nextWave++;
            if (nextWave <= bosswave)
            {

              Destroy(GameObject.FindGameObjectWithTag("Player"));
                gameManager.GetComponent<GameManager>().InstantiatePlayer();

            }
        }

    }
    void SpawnEnemy(Transform _enemy)
    {
        Transform _spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(_enemy, _spawnPoint.position, _spawnPoint.rotation);
    }
    public int fibonacci(int n)
    {

        if (n == 0)
        {
            return 0;
        }
        else if (n == 1)
        {
            return 1;
        }
        else
        {
            return fibonacci(n - 1) + fibonacci(n - 2);
        }
    }
    public List<float> getWavesStat() 
    {
        return WavesDuration;
    }
    public int getWavesCompleted() 
    {
        return nextWave-1;
    }
    [System.Serializable]
    public class Wave
    {

        public Transform[] enemies;
        public float rate;

    }

    void Wining() { waveswon += (nextWave - 1); gameManager.GetComponent<GameManager>().Level(level);   string character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().GetName(); Character.SaveStat(character, Character.MAX_WAVES, waveswon);  }

}
