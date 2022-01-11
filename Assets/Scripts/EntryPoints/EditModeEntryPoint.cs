using System.Collections.Generic;
using UnityEngine;

public class EditModeEntryPoint : MonoBehaviour
{
    [SerializeField] private List<EntryPointSystemBase> systems;

    private void Awake()
    {
        foreach (var s in systems)
        {
            s.Begin();
        }
    }
}
