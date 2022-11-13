using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using UniRx;
using System.Linq;

/// <summary>
/// ProjectContextにアタッチされるグローバルなInstaller.
/// </summary>
public class ApplicationInstaller : MonoInstaller
{
    [SerializeField] int targetFrameRate = 60;
    [SerializeField] TransitionController transitionController;

    public override void InstallBindings()
    {
        Application.targetFrameRate = targetFrameRate;

        Container.Bind<TransitionController>().FromInstance(transitionController).AsSingle();

        //TODO: 即時プレイしたシーンのisLoadedフラグをすぐ立てないと次のシーンが読み込まれない。きれいな方法考える
        var sceneBase = GameObject.FindObjectsOfType<SceneBase>().FirstOrDefault(x => !x.IsLoaded);
        if (sceneBase != null)
        {
            sceneBase.IsLoaded = true;
        }
    }
}