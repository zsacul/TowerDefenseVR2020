using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellNode
{
    Dictionary<(int, int), ISpellNode> GetChildren();
}
