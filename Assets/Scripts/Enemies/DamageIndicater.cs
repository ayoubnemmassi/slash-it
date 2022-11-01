using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicater : MonoBehaviour
{
    public Text text;
    public float lifetime = 0.6f;
    public float minDist = 2f;
    public float maxDist = 3f;
    Vector3 initialPos;
    Vector3 targetPos;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        float direction = Random.rotation.eulerAngles.y;
        initialPos = transform.position;
      


        float dist = Random.Range(minDist, maxDist);
       
        targetPos = initialPos + (Quaternion.Euler(0, direction, 0 ) * new Vector3(dist, dist, 0f));
        if (targetPos.y < 4) { targetPos += new Vector3(0, 4 - targetPos.y, 0); }
    
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float fraction = lifetime / 2f;
        if (timer > lifetime) { Destroy(gameObject); }
        else if (timer > fraction) { text.color = Color.Lerp(text.color, Color.clear, (timer - fraction) / (lifetime)); }
    //    print("target : "+targetPos);
        
        transform.position = Vector3.Lerp(initialPos, targetPos, Mathf.Sin(timer / lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Sin(timer / lifetime));
    }

    public void SetDamageText(int damage) 
    {
        text.text = damage.ToString();
    }
}
