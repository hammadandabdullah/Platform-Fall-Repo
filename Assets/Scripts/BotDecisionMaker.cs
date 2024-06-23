using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDecisionMaker : MonoBehaviour
{
    private InputHandler inputHandler;
    private NearbyPlatformChecker nearbyPlatformChecker;
    private NearbyPlayerChecker nearbyPlayerChecker;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform gapChecker;
    [SerializeField] private float gapCheckerLength = 5f;

    private Transform platformToGo;
    private float maxDistanceFromPlatformToGo = 2f;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        nearbyPlatformChecker = GetComponent<NearbyPlatformChecker>();
        nearbyPlayerChecker = GetComponent<NearbyPlayerChecker>();

        FindActivePlatform();
    }

    
    private void Update()
    {
        if(platformToGo != null)
        {
            //Rotation
            Vector3 direction = platformToGo.position - transform.position;
            direction.Normalize();

            float step = rotationSpeed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            //Movement
            bool canGoForward = Vector3.Distance(transform.position, platformToGo.position) > maxDistanceFromPlatformToGo;
            inputHandler.SetBotMovement(canGoForward);

            //Jump
            bool foundGap = !Physics.Raycast(gapChecker.position, -gapChecker.up, gapCheckerLength);
            inputHandler.SetBotJump(foundGap);

            //Find New Platform
            if (!canGoForward)
            {
                FindActivePlatform();
            }

            //Push
            bool isPlayerInRange = nearbyPlayerChecker.IsPlayerInRange();
            bool shouldPush = Random.Range(0, 2) == 0;

            if (shouldPush)
                inputHandler.SetBotPush(isPlayerInRange);
            else
                inputHandler.SetBotBlock(isPlayerInRange);
        }
    }

    float delayToFindNewPlatform = 0.2f;
    private void FindActivePlatform()
    {
        platformToGo = nearbyPlatformChecker.GetPlatform();

        if (platformToGo == null)
        {
            Invoke(nameof(FindActivePlatform), delayToFindNewPlatform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(gapChecker.position, -gapChecker.up * gapCheckerLength);
    }
}
