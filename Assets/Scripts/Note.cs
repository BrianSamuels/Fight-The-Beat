using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;

    //Time the note is supposed to be tapped by the player
    public float assignedTime;

    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        //used to be *2
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 1.2));

        
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //(spawnY = 0) (despawnY = 1) (spawn < t < despawn)
            transform.localPosition = Vector3.Lerp(Vector3.forward * SongManager.Instance.noteSpawnZ, Vector3.forward * SongManager.Instance.noteDespawnZ, t);
            //GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
