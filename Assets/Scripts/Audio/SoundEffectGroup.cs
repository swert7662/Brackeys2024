using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "NewSoundEffectGroup", menuName = "Audio/SoundEffectGroup")]
public class SoundEffectGroup : ScriptableObject
{
    public string groupName; // e.g. "Footsteps"
    public SoundEffect[] soundEffects; // Array of SoundEffect instances for the group

    [Header("Volume Settings")]
    public bool RandomizeGroupVolume = false;
    [ShowIf("RandomizeGroupVolume")]
    [MinMaxSlider(0f, 1f, showFields: true)] 
    public Vector2 volumeRange = new Vector2(1f, 1f);

    [Header("Pitch Settings")]
    public bool RandomizeGroupPitch = false;
    [ShowIf("RandomizeGroupPitch")]
    [MinMaxSlider(0.1f, 3f, showFields: true)]
    public Vector2 pitchRange = new Vector2(1f, 1f);
}
