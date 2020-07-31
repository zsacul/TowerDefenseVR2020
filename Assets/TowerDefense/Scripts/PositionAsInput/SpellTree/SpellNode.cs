using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellNode
{
    public Dictionary<Vector2Int,SpellNode> children;
    public Spell toCast;
    // Start is called before the first frame update
    public SpellNode()
    {
        children = new Dictionary<Vector2Int, SpellNode>();
    }

}
