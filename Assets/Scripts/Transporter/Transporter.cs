using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _from = null;
    [SerializeField] private MonoBehaviour _to = null;

    [SerializeField] private float _movementTime = 1f;

    private float _movementProgress = 0f;
    private ResourceType _holdingResource;
    private IEnumerator _movement = null;

    private IResourceStorage From => (IResourceStorage)_from;
    private IResourceStorage To => (IResourceStorage)_to;

    private void OnEnable()
    {
        From.ResourceAdded += _ => StartMovement();
    }

    private void OnDisable()
    {
        From.ResourceAdded -= _ => StartMovement();
    }

    private void OnValidate()
    {
        if (_from is IResourceStorage && _to is IResourceStorage) return;

        if (_from is IResourceStorage == false)
        {
            Debug.LogError($"{nameof(_from)} should implement {nameof(IResourceStorage)}");
            _from = null;
        }

        if (_to is IResourceStorage == false)
        {
            Debug.LogError($"{nameof(_to)} should implement {nameof(IResourceStorage)}");
            _to = null;
        }
    }

    private void StartMovement()
    {
        if (_movement == null)
        {
            if (From.Capacity == 0) return;
            if (To.Capacity == To.MaxCapacity) return;

            _holdingResource = From.Containings[0].Resource;
            From.Remove(_holdingResource);
            _movement = Movement(_movementTime);
            StartCoroutine(_movement);
        }
    }


    // вот бы эту ебаную корутину отдельно запхать для реюза, даааа.
    private IEnumerator Movement(float time)
    {
        _movementProgress = 0f;

        while(_movementProgress < time)
        {
            _movementProgress += Time.deltaTime;
            yield return null;
        }

        To.Add(_holdingResource);
        _movement = null;
        StartMovement();
    }
}
