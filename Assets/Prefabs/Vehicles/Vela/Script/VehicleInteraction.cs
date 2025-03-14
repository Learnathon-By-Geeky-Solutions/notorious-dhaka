using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    public GameObject player;
    private GameObject currentVehicle;
    private bool canEnterVehicle = false;
    public bool isInVehicle = false;

    private PlayerMovement playerMovement;
    private RaftController raftController;
    private Rigidbody playerRb;

    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (canEnterVehicle && Input.GetKeyDown(KeyCode.Return))
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
        player.transform.SetParent(currentVehicle.transform);

        Vector3 seatPosition = currentVehicle.transform.position + new Vector3(0, -1, 0);
        player.transform.position = seatPosition;
        player.transform.rotation = currentVehicle.transform.rotation;

        playerMovement.enabled = false;
        if (playerRb != null)
        {
            playerRb.isKinematic = true; 
        }

        raftController = currentVehicle.GetComponent<RaftController>();
        if (raftController != null)
        {
            raftController.EnableControl(true);
        }

        isInVehicle = true;
        Debug.Log("Entered the vehicle!");
    }

    void ExitVehicle()
    {
        player.transform.SetParent(null);
        Vector3 exitPosition = currentVehicle.transform.position + new Vector3(3, 1, 0);
        player.transform.position = exitPosition;

        playerMovement.enabled = true;
        if (playerRb != null)
        {
            playerRb.isKinematic = false; 
        }

        if (raftController != null)
        {
            raftController.EnableControl(false);
        }

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
        if (other.CompareTag("Vehicle") && currentVehicle == other.gameObject)
        {
            currentVehicle = null;
            canEnterVehicle = false;
        }
    }
}
