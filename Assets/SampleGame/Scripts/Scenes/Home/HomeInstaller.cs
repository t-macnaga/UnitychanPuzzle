using UnityEngine;
using Zenject;
using UniRx;

public class HomeInstaller : MonoInstaller<HomeInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IMessageBroker>().To<MessageBroker>().AsSingle();
    }
}