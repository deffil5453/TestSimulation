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
    void Start()
    {
        if (gameObject.CompareTag("BlueDrone"))
        {
            HomeBase = GameObject.FindGameObjectWithTag("BlueBase").transform;
        }
        else
        {
            HomeBase = GameObject.FindGameObjectWithTag("RedBase").transform;
        }
            //HomeBase = GameObject.FindGameObjectWithTag();
            _navMeshDrone = GetComponent<NavMeshAgent>();
        SearchResource();
    }

    void Update()
    {
        if (TargetResource == null && !_hasResource)
        {
            SearchResource();
        }
        if (!_navMeshDrone.pathPending &&_navMeshDrone.remainingDistance <= _navMeshDrone.stoppingDistance)
        {
            if (_hasResource)
            {

            }
            else
            {
                CollectResource();
            }
        }
    }

    private void SearchResource()
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag("Crystal");
        float closestDistance = Mathf.Infinity;
        foreach (var resource in resources)
        {
            float distance = Vector3.Distance(transform.position, resource.transform.position);
            if (distance < closestDistance)
            {
                TargetResource = resource;
                closestDistance = distance;
            }
        }
        _navMeshDrone.SetDestination(TargetResource.transform.position);
    }
    private void CollectResource()
    {
        _time += Time.deltaTime;
        if (_time <= _collectionTime)
        {
            Destroy(TargetResource);
            _hasResource = true;
            _time = 0;
            _navMeshDrone.SetDestination(HomeBase.position);
        }
    }
}
