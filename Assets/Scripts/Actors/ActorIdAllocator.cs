using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorIdAllocator : MonoBehaviour
{
    List<uint> bitmap_ = new List<uint>();

    public uint GetId()
    {
        return 0;
    }

    public void ReleaseId(uint id)
    {

    }
}
