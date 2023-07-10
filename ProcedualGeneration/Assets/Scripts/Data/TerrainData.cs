using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
    public float uniformScale = 1f;
    public bool useFlatShading;
    public bool useFalloffMap;
    public float meshHeightMultipier;
    public AnimationCurve meshHeightCurve;

    public float minHeight{
        get{
            return uniformScale * meshHeightMultipier * meshHeightCurve.Evaluate(0);
        }
    }

    public float maxHeight{
        get{
            return uniformScale * meshHeightMultipier * meshHeightCurve.Evaluate(1);
        }
    }
}
