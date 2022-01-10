using System.Collections.Generic;
using UnityEngine;

public class EditModeEntryPoint : MonoBehaviour
{
    [SerializeField] private List<EntryPointSystemBase> systems;

    private void Awake()
    {
        for (var i = 0; i < systems.Count; i++)
        {
            systems[i].Begin();
        }
    }
}
