using UnityEngine;


public class CapsuleMessageForwarding : MonoBehaviour
{

    public GameObject MessageReceiver;

    public Transform RootForPlayer;

    public void AnimFinished()
    {
        if (MessageReceiver != null)
            MessageReceiver.SendMessage("AnimFinished");
    }
}

