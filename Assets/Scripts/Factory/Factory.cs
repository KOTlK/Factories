using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private int _storageCapacity;

    private IResourceStorage _storage;

    private void Start()
    {
        _storage = new ResourcesStorage(_storageCapacity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"{_storage.TryAdd(ResourceType.First, 30)}, {_storage.Capacity}");
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log($"{_storage.TryRemove(ResourceType.First, 30)}, {_storage.Capacity}");
        }
    }
}
