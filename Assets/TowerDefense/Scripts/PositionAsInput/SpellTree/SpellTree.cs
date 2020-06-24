using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellTree
{
    private SpellNode root;
    private SpellNode step;
    // Start is called before the first frame update
    public SpellTree(Spell[] spells)
    {
        root = new SpellNode();
        step = root;
        foreach(Spell spell in spells)
        {
            BuildTree(spell);
        }
    }
    private void BuildTree(Spell spell)
    {
        SpellNode current = root;
        for(int i = 0; i < spell.castSequence.Length; i++)
        {
            if(current.children.ContainsKey(spell.castSequence[i]))
            {
                current = current.children[spell.castSequence[i]];
            }
            else
            {
                SpellNode node = new SpellNode();
                current.children.Add(spell.castSequence[i], node);
                current = node;
            }
        }
        current.toCast = spell;
    }
    public void BeginCast()
    {
        step = root;
    }
    public (bool,Spell) CastStep(Vector2Int cast)
    {
        if(step.children.ContainsKey(cast))
        {
            step = step.children[cast];
            return (true,step.toCast);
        }
        else
        {
            return (false,null);
        }
    }
    public Vector2Int[] GetNodes()
    {
        return step.children.Keys.ToArray();
    }
}
