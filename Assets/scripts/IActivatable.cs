using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
    string TooltipMessage { get; }
    void Activate();
}
