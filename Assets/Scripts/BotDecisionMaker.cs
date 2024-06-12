using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDecisionMaker : MonoBehaviour
{
    private InputHandler inputHandler;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform gapChecker;
    [SerializeField] private float gapCheckerLength = 5f;

    private Transform platformToGo;
    private NearbyPlatformChecker nearbyPlatformChecker;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        nearbyPlatformChecker = GetComponent<NearbyPlatformChecker>();

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
            bool canGoForward = Vector3.Distance(transform.position, platformToGo.position) > 0.5f;
            inputHandler.SetBotMovement(canGoForward);

            //Jump
            bool foundGap = !Physics.Raycast(gapChecker.position, -gapChecker.up, gapCheckerLength);
            inputHandler.SetBotJump(foundGap);

            //Find New Platform
            if (!canGoForward)
            {
                FindActivePlatform();
            }
        }
    }

    float delayToFindNewPlatform = 2f;
    private void FindActivePlatform()
    {
        VibratingPlatform platform = nearbyPlatformChecker.GetPlatform();
        if(platform == null)
        {
            Invoke(nameof(FindActivePlatform), delayToFindNewPlatform);
        }
        else if (platform.IsActive())
        {
            platformToGo = platform.transform;
        }
        else
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
