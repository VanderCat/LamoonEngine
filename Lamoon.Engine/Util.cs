using System.Numerics;
using Lamoon.Data;
using Lamoon.Filesystem;
using NekoLib.Core;
using NekoLib.Scenes;
using Serilog;
using BindingFlags = System.Reflection.BindingFlags;

namespace Lamoon.Engine; 

public static class Util {
    public static IScene CreateScene(SceneDefinition sceneDefinition) {
        var sceneType = Type.GetType(sceneDefinition.Type);
        if (sceneType is null)
            throw new ArgumentException($"The scene type {sceneDefinition.Type} has not been found");
        if (!sceneType.IsAssignableTo(typeof(IScene)))
            throw new ArgumentException($"The type {sceneDefinition.Type} does not implement IScene");
        var sceneConstructor = sceneType.GetConstructor(new[]{typeof(SceneDefinition)});
        if (sceneConstructor is null)
            throw new ArgumentException($"{sceneDefinition.Type} does not support deserialization");
        var scene = (IScene) sceneConstructor.Invoke(new object?[] {sceneDefinition});
        return scene;
    }

    public static void LoadSceneFromFilesystem(string path) {
        var file = Files.GetFile(path);
        using var stream = file.GetStream();
        var definition = Definition.FromStream<SceneDefinition>(stream);
        SceneManager.LoadScene(CreateScene(definition));
    }
    
    private static Type? FindType(string fullName)
    {
        return
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName.Equals(fullName));
    }

    public static List<GameObject> CreateGameObjectsFromDefinition(GameObjectDefinition definition, GameObject? parent = null) {
        var type = FindType(definition.Type);
        if (type is null)
            throw new ArgumentException($"The gameObject type {definition.Type} has not been found");
        if (!type.IsAssignableTo(typeof(GameObject)))
            throw new ArgumentException($"The type {type} is not assignable to GameObject");
        var constructor = type.GetConstructor(new[]{typeof(string)});
        if (constructor is null)
            throw new ArgumentException($"{type} does not have default constructor");
        var gameObjects = new List<GameObject>();
        var gameObject = (GameObject) constructor.Invoke(new object?[] {definition.Name});
        gameObject.Id = definition.Id;
        gameObject.Transform.LocalPosition = definition.Transform.Position;
        gameObject.Transform.LocalRotation = Quaternion.CreateFromYawPitchRoll(definition.Transform.Rotation.X, definition.Transform.Rotation.Y, definition.Transform.Rotation.Z);
        gameObject.Transform.LocalScale = definition.Transform.Scale;
        if (definition.Components is not null)
            CreateComponentsFromDefinition(gameObject, definition.Components);
        if (parent is not null)
            gameObject.Transform.Parent = parent.Transform;
        if (definition.Children is not null)
            foreach (var childDefinition in definition.Children) {
                var childGameObjects = CreateGameObjectsFromDefinition(childDefinition, gameObject);
                gameObjects.AddRange(childGameObjects);
            }
        gameObjects.Add(gameObject);
        return gameObjects;
    }

    public static void CreateComponentsFromDefinition(GameObject gameObject, List<ComponentDefinition> definitions) {
        foreach (var definition in definitions) {
            var type = FindType(definition.Type);
            if (type is null) {
                Log.Error("The component of type {Type} has not been found", definition.Type);
                break;
            }
            if (!type.IsAssignableTo(typeof(Component))) {
                Log.Error("The type {Type} is not a Component", type);
                break;
            }
            var constructor = type.GetConstructor(new Type[]{});
            if (constructor is null) {
                Log.Error("{Type} does not have default constructor", type);
                break;
            }
            //fixme: this should be made better?
            var component = (Component)constructor.Invoke(new object?[] {});
            ((List<Component>)gameObject.GetType().GetField("_components", 
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(gameObject)).Add(component);
            component
                .GetType()
                .GetProperty("GameObject", BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .GetSetMethod(true)
                .Invoke(component, new object?[]{gameObject});
            if (gameObject.Initialized)  {
                component.Invoke("Awake");
                component.GetType().GetField("_awoke").SetValue(component, true);
            }
            if (definition.Fields is not null)
                foreach (var field in definition.Fields) {
                    component.GetType().GetField(field.Key).SetValue(component, field.Value);
                }
        }
    }
}