using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [field: SerializeField] public BGMHandler BGMHandler { get; private set; }

    [Header("Volume")]
    [Range(0, 1)]
    public float MasterVolume = 1;

    [Range(0, 1)]
    public float MusicVolume = 1;

    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters; //all objects require Studio Event Emitter component

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }

        Instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Update()
    {
        masterBus.setVolume(MasterVolume);
        musicBus.setVolume(MusicVolume);
        sfxBus.setVolume(SFXVolume);
    }
    
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance Create2DEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public EventInstance Create3DEventInstance(EventReference eventReference, Vector3 worldPos)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        UpdateEventInstanceAttributes(eventInstance, worldPos);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    public void PauseAllSFX()
    {
        sfxBus.setPaused(true);
    }

    public void UnpauseAllSFX()
    {
        sfxBus.setPaused(false);
    }

    public void UpdateEventInstanceAttributes(EventInstance eventInstance, Vector3 worldPos)
    {
        //Use Unity FMod built in runtime utils to get attributes (for 3D timelines)
        FMOD.ATTRIBUTES_3D attributes = RuntimeUtils.To3DAttributes(worldPos);

        eventInstance.set3DAttributes(attributes);
    }

    public void StopEventInstance(EventInstance eventInstance, FMOD.Studio.STOP_MODE stopMode)
    {
        if (eventInstances.Contains(eventInstance))
        {
            eventInstance.stop(stopMode);
        }
    }

    private void CleanUp()
    {
        //stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        //stop all of the event emitters
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
