using System;

[Serializable]
public class ResourceCell
{
    public ResourceType Resource;
    public int Amount;

    public ResourceCell(ResourceType resource)
    {
        Resource = resource;
        Amount = 1;
    }

    public ResourceCell(ResourceType resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }

    public bool Equals(ResourceCell cell)
    {
        return Resource == cell.Resource && Amount == cell.Amount;
    }
}
