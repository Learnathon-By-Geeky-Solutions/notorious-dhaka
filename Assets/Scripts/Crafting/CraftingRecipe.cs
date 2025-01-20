using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables;

namespace Crafting
{
    [CreateAssetMenu]
    public class CraftingRecipe : ScriptableObject
    {
        public List<Items> inputItems;
        public Items outputItem;
    }
}
