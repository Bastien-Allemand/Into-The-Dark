using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Member;

[InitializeOnLoad]
public static class AudioSourceWatcher
{
    static AudioSourceWatcher()
    {
        ObjectFactory.componentWasAdded += OnComponentAdded;
    }

    private static void OnComponentAdded(Component component)
    {
        if (component is AudioSource audioSource)
        {
            component.gameObject.AddComponent<SoundOutil>();
        }
    }
}
