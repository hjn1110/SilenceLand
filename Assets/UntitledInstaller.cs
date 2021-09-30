using UnityEngine;
using Zenject;

public class UntitledInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<string>().FromInstance("Hello World!");
        Container.Bind<Greeter>().AsSingle().NonLazy();


        Container.Bind<ISeeComponent>().To<SeeComponent>().AsTransient();



    }
}

public class Greeter
{
    public Greeter(string message)
    {
        Debug.Log(message);
    }
}