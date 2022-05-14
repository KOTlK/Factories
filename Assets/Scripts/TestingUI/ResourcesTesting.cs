using UnityEngine;
using UnityEngine.UI;

public class ResourcesTesting : MonoBehaviour
{
    [SerializeField] private Text _incoming;
    [SerializeField] private Text _outcoming;
    [SerializeField] private Text _collected;

    public void UpdateUI(ResourceStorage incoming, ResourceStorage outcoming, ResourceStorage collected)
    {
        string incomingText = "";
        string outcomingText = "";
        string collectedText = "";

        foreach(ResourceCell i in incoming)
        {
            incomingText += $"{i.Resource} - {i.Amount} ";
        }

        foreach (ResourceCell i in outcoming)
        {
            outcomingText += $"{i.Resource} - {i.Amount} ";
        }

        foreach (ResourceCell i in collected)
        {
            collectedText += $"{i.Resource} - {i.Amount} ";
        }

        _incoming.text = incomingText;
        _outcoming.text = outcomingText;
        _collected.text = collectedText;

    }
}
