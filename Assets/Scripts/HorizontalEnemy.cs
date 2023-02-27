using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    public Transform[] moveSpot;
    public int curSpot = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot[curSpot].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpot[curSpot].position) < 0.1f)
        {
            curSpot = (curSpot + 1) % moveSpot.Length;
        }
    }
}
