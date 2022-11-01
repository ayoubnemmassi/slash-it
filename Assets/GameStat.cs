using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStat : MonoBehaviour
{
    [SerializeField] Spawning spawning;
    [SerializeField] TextMeshProUGUI wavesCompletedText;
    [SerializeField] TextMeshProUGUI wavesStatText;
    // Start is called before the first frame update
    void Start()
    {
        wavesCompletedText.text = spawning.getWavesCompleted().ToString();
        print(spawning.getWavesCompleted());
        wavesStatText.text = "";
        for (int i = 0; i < spawning.getWavesStat().Count; i++) 
        {
           wavesStatText.text+=i+1+" : "+ spawning.getWavesStat()[i]+" s\n";
        }
        
            
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
