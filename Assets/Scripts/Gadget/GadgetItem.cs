using UnityEngine;

public class GadgetItem : MonoBehaviour
{
    private GadgetData myData;
    private GadgetSocket mySocket;

    public void Setup(GadgetData data, GadgetSocket socket)
    {
        myData = data;
        mySocket = socket;
    }

    private void OnDestroy()
    {
        if (myData != null) myData.currentInstances--;
        if (mySocket != null) mySocket.isOccupied = false;
    }
}