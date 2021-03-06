using System.Collections.Generic;
using System.Linq;
using SharpEngine.Components;
using SharpEngine.Utils;

namespace SharpEngine.Entities;

/// <summary>
/// Entité basique
/// </summary>
public class Entity
{
    internal Scene Scene;
    private readonly List<Component> _components;

    public Entity()
    {
        _components = new List<Component>();
        Scene = null;
    }

    public List<Component> GetComponents() => _components;

    public T AddComponent<T>(T comp) where T: Component
    {
        comp.SetEntity(this);
        _components.Add(comp);
        return comp;
    }

    public T GetComponent<T>() where T: Component
    {
        return _components.Where(comp => comp.GetType() == typeof(T)).Cast<T>().FirstOrDefault();
    }

    public void RemoveComponent(Component component)
    {
        component.SetEntity(null);
        _components.Remove(component);
    }

    public virtual void SetScene(Scene scene) => Scene = scene;
    public Scene GetScene() => Scene;

    public virtual void Initialize() 
    {
        foreach (var comp in _components)
            comp.Initialize();
    }

    public virtual void LoadContent()
    {
        foreach (var comp in _components)
            comp.LoadContent();
    }

    public virtual void UnloadContent()
    {
        foreach (var comp in _components)
            comp.UnloadContent();
    }

    public virtual void TextInput(object sender, Key key, char character)
    {
        foreach (var comp in _components)
            comp.TextInput(sender, key, character);
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var comp in _components)
            comp.Update(gameTime);
    }

    public virtual void Draw(GameTime gameTime)
    {
        foreach (var comp in _components)
            comp.Draw(gameTime);
    }
}
