using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
  // [SerializeField] Character character;
   [SerializeField] Text stattext;   
    // [SerializeField] Character character;
    [SerializeField] TextMeshProUGUI maxWaves;
    [SerializeField] TextMeshProUGUI GamesPlayed;
    [SerializeField] GameObject medaille;
    // Start is called before the first frame update
    void Start()
    {
        if (Character.GetMaxWaves(gameObject.name) > 0) { medaille.SetActive(true); }
        else { medaille.SetActive(false); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void getstats() 
    {
        maxWaves.text= Character.GetMaxWaves(gameObject.name).ToString();
        GamesPlayed.text= Character.GetPlayedGames(gameObject.name).ToString();
    }
}
