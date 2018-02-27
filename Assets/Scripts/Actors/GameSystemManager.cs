using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class GameSystemManager
{
    List<GameSystem> systems_ = new List<GameSystem>();

    public void AddSystem(GameSystem sys)
    {
        if (systems_.Contains(sys))
        {
            Debug.LogError("System is already added: " + sys.GetType().ToString());
            return;
        }

        systems_.Add(sys);
        systems_ = systems_.OrderBy(s => s.Priority).ToList();
    }

    public void RemoveSystem(GameSystem sys)
    {
        if (!systems_.Contains(sys))
        {
            Debug.LogError("System not found: " + sys.GetType().ToString());
            return;
        }

        systems_.Remove(sys);
    }

    public void Update()
    {
        for (int i = 0; i < systems_.Count; ++i)
        {
            systems_[i].Tick();
        }
    }
}
