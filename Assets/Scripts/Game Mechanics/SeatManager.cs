using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public int CountFilledSeats()
    {
        // Get the parent of this object
        Transform parentTransform = transform.parent;

        // If there is no parent, return 0 as no siblings exist
        if (parentTransform == null)
        {
            return 0;
        }

        // Initialize a hash set to store unique filled seats
        HashSet<Transform> uniqueFilledSeats = new HashSet<Transform>();

        // Loop through each sibling (and self) in the parent
        foreach (Transform sibling in parentTransform)
        {
            // Check if the sibling has the tag "Seat" and a child
            if (sibling.CompareTag("BoatSeat") && sibling.childCount > 0)
            {
                // Add the sibling to the set if it has a child
                uniqueFilledSeats.Add(sibling);
            }
        }

        // Return the number of unique filled seats
        return uniqueFilledSeats.Count;
    }

    // Example method that triggers when object is clicked
    public void OnSeatCheck()
    {
        int filledSeatCount = CountFilledSeats();
        Debug.Log("Number of filled seats: " + filledSeatCount);
    }
}
