using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        NPCStateController npc = other.GetComponent<NPCStateController>();
        if (npc != null)
        {
            npc.SetInZone(true); // Set the NPC to InZone state when it enters the zone
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NPCStateController npc = other.GetComponent<NPCStateController>();
        if (npc != null)
        {
            npc.SetInZone(false); // Reset the NPC state when it leaves the zone
        }
    }
}
