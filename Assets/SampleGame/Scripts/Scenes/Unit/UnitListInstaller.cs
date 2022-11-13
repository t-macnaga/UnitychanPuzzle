using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using Zenject;

public class UnitListInstaller : MonoInstaller<UnitListInstaller>
{
    [SerializeField] UnitCell cellPrototype = default;
    [SerializeField] Transform cellParent = default;

    public override void InstallBindings()
    {
        Container.BindFactory<UnitCell, UnitCell.Factory>()
        .FromComponentInNewPrefab(cellPrototype)
        .WithGameObjectName("UnitCell")
        .UnderTransform(cellParent);

        cellPrototype.gameObject.SetActive(false);
    }
}