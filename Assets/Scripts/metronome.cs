using UnityEditor.TerrainTools;
using UnityEngine;

public class metronome : MonoBehaviour
{
    
    [SerializeField] private MIDILoader loader;

    private float beatInterval;
    private float cosY;
    private float currentTime;
    private float startTime;
    private AudioSource metronomeClick;
    private float lastMetronomeTime;
    private float BPM;



    private void Start()
    {
        startTime = Time.time;
        metronomeClick = GetComponent<AudioSource>();
        BPM = loader.GetBPM();
        Debug.Log(BPM);
    }

    void Update()
    {
        
        currentTime += Time.deltaTime;
        beatInterval = 60f / BPM;

        if (currentTime >= beatInterval)
        {
            lastMetronomeTime = currentTime;
            currentTime = 0f;
            metronomeClick.Play();
            
        }
    }

    public float GetLastMetronomeTime()
    {
        return lastMetronomeTime;
    }
}
