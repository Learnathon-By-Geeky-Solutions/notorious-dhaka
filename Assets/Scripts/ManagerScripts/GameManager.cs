using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameStarted;
    public UnityEvent OnPunchPerformed;

    private void Awake()
    {
        if (Instance == null)
        Instance = this;
        else
            Destroy(Instance);
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
