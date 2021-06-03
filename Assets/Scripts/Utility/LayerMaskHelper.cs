using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskHelper
{
    public static bool Contains(LayerMask layer, GameObject gameObject)
    {
        return ((1 << gameObject.layer) & layer) != 0;
    }
}
