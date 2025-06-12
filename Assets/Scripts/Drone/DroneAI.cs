using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public Transform HomeBase;
    public GameObject TargetResource;

    [SerializeField] private float _collectionTime = 2f;
    [SerializeField] private float _time = 0f;

    [SerializeField] private NavMeshAgent _navMeshDrone;

    [SerializeField] private bool _hasResource = 1 == 3;

    [SerializeField] private float _avoidRadius = 3f;
    [SerializeField] private float _avoidForce = 1f;
    void Start()
    {
        _navMeshDrone = GetComponent<NavMeshAgent>();
        if (gameObject.CompareTag("BlueDrone"))
            HomeBase = GameObject.FindGameObjectWithTag("BlueBase").transform;
        else
            HomeBase = GameObject.FindGameObjectWithTag("RedBase").transform;

        SearchResource();
    }

    void Update()
    {
        AvoidDrone();
        if (TargetResource == null && !_hasResource)
        {
            SearchResource();
        }
        if (!_navMeshDrone.pathPending && _navMeshDrone.remainingDistance <= _navMeshDrone.stoppingDistance)
        {
            if (_hasResource)
                UnloadResource();
            else
                CollectResource();
        }
    }

    private void SearchResource()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Crystal");
        float closestDistance = Mathf.Infinity;
        GameObject closestResource = null;
        if (resources.Length > 0)
        {
            foreach (var resource in resources)
            {
                float distance = Vector3.Distance(transform.position, resource.transform.position);
                if (distance < closestDistance)
                {
                    closestResource = resource;
                    closestDistance = distance;
                }
            }
            TargetResource = closestResource;
            _navMeshDrone.SetDestination(TargetResource.transform.position);
        }
    }
    private void CollectResource()
    {
        _navMeshDrone.ResetPath();
        _time += Time.deltaTime;
        if (_time >= _collectionTime)
        {
            Destroy(TargetResource);
            _hasResource = true;
            _time = 0;
            _navMeshDrone.SetDestination(HomeBase.position);
        }
    }
    private void UnloadResource()
    {
        _hasResource = false;
        SearchResource();
    }
    private void AvoidDrone()
    {
        Vector3 avoidanceVector = Vector3.zero;
        int nearbyDronesCount = 0;

        GameObject[] drones = GameObject.FindGameObjectsWithTag(gameObject.tag);
        foreach (var drone in drones)
        {
            if (drone != gameObject)
            {
                float distance = Vector3.Distance(transform.position, drone.transform.position);
                if (distance < _avoidRadius)
                {
                    Vector3 away = transform.position - drone.transform.position;
                    if (distance > 0.01f)
                    {
                        away /= distance; // нормализация и взвешивание по расстоянию
                    }
                    avoidanceVector += away;
                    nearbyDronesCount++;
                }
            }
        }
        if (nearbyDronesCount > 0)
        {
            avoidanceVector /= nearbyDronesCount;
            Vector3 desiredPosition;
            if (_hasResource)
            {
                // Цель - база, смещаем точку базы
                desiredPosition = HomeBase.position + avoidanceVector * _avoidForce;
            }
            else if (TargetResource != null)
            {
                // Цель - ресурс, смещаем точку ресурса
                desiredPosition = TargetResource.transform.position + avoidanceVector * _avoidForce;
            }
            else
            {
                desiredPosition = transform.position + avoidanceVector * _avoidForce;
            }
            _navMeshDrone.SetDestination(desiredPosition);
        }
    }
}