using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public GameObject player; // Reference to the player
    private GameObject currentVehicle; // The vehicle player can enter
    private bool canEnterVehicle = false;
    public bool isInVehicle = false;


    // Reference to the player's movement script (assuming it's named 'PlayerMovement')
    private PlayerMovement playerMovement;

    void Start()
    {
        // Get the player movement script on start
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    void Update()
    {
        if (canEnterVehicle && Input.GetKeyDown(KeyCode.Return)) // Return is the Enter key
        {
            if (!isInVehicle)
            {
                EnterVehicle();
            }
            else
            {
                ExitVehicle();
            }
        }
    }

    void EnterVehicle()
    {
        // Attach player to the vehicle
        player.transform.SetParent(currentVehicle.transform);
        player.transform.localPosition = Vector3.zero; // Adjust position to the vehicle seat
        player.transform.rotation = currentVehicle.transform.rotation;

        // Disable player movement
        playerMovement.enabled = false;

        // Enable vehicle movement (you can add the code for vehicle movement here)
        isInVehicle = true;
        Debug.Log("Entered the vehicle!");
    }

    void ExitVehicle()
    {
        // Detach player from the vehicle
        player.transform.SetParent(null);
        player.transform.position = currentVehicle.transform.position + new Vector3(3, 0, 0); // Set exit position

        // Re-enable player movement
        playerMovement.enabled = true;

        isInVehicle = false;
        Debug.Log("Exited the vehicle!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            currentVehicle = other.gameObject;
            canEnterVehicle = true;
            Debug.Log("Press Enter to get into the vehicle.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            if (currentVehicle == other.gameObject)
            {
                currentVehicle = null;
                canEnterVehicle = false;
            }
        }
    }
}
