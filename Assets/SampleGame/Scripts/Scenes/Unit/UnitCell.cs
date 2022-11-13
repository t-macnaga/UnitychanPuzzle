using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnitCell : MonoBehaviour
{
    public class Factory : PlaceholderFactory<UnitCell>
    {

    }

    [SerializeField] Text text;

    public void Setup(UnitEntity unit)
    {
        Debug.Log("Construct");
        // this.unit = unit;
        text.text = unit.name;
    }

    // void Awake()
    // {
    // }
}
