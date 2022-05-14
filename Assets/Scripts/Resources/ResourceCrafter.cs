using System.Threading.Tasks;
using System;
using UnityEngine;

public class ResourceCrafter
{
    public event Action CraftStarted;
    public event Action CraftCompleted;
    public event Action<float> CraftProgressUpdated;

    public bool InProcess { get; private set; }

    public async Task<ResourceCell> Craft(ResourceBlueprint blueprint, IResourceStorage incomeStorage)
    {
        InProcess = true;
        CraftStarted?.Invoke();
        if (blueprint.Income.Length > 0)
        {
            foreach (var cell in blueprint.Income)
            {
                var amount = cell.Amount;
                while (amount > 0)
                {
                    incomeStorage.Remove(cell.Resource);
                    amount--;
                }
            }
        }
        

        await Wait(blueprint.CraftTime);

        InProcess = false;
        CraftCompleted?.Invoke();
        return blueprint.Outcome;
    }

    private async Task Wait(float seconds)
    {
        var estimatedTime = 0f;

        while (estimatedTime < seconds)
        {
            var waitingTime = Time.deltaTime;
            estimatedTime += waitingTime;
            CraftProgressUpdated?.Invoke(estimatedTime / seconds);

            await Task.Delay((int)(waitingTime * 1000));
        }
        CraftProgressUpdated?.Invoke(1f);
        CraftProgressUpdated?.Invoke(0f);
    }
}
