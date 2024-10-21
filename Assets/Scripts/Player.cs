using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int score;

    [SerializeField]
    private int speed;

    private const int NORMAL_SPEED = 5;
    private const int HIGH_SPEED = 10;

    // Start is called before the first frame update
    void Start()
    {
        speed = NORMAL_SPEED;
    }

    // Update is called once per frame
    void Update()
    {
        score += (int) Time.deltaTime;
    }
}
