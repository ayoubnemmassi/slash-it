using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class duration : MonoBehaviour

{
    TextMeshProUGUI text;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        text.text = timer.ToString();

    }
}
