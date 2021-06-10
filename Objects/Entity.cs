using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenGL_Game.Components;
using OpenTK;

namespace OpenGL_Game.Objects
{
    class Entity
    {
        string name;
        List<IComponent> componentList = new List<IComponent>();
        ComponentTypes mask;
        Matrix4 model;
        Vector2 EndNode;
        bool ChaseFinish;
 
        public Entity(string name)
        {
            this.name = name;
        }

        /// <summary>Adds a single component</summary>
        public void AddComponent(IComponent component)
        {
            Debug.Assert(component != null, "Component cannot be null");

            componentList.Add(component);
            mask |= component.ComponentType;
        }

        public String Name
        {
            get { return name; }
        }

        public ComponentTypes Mask
        {
            get { return mask; }
        }

        public List<IComponent> Components
        {
            get { return componentList; }
        }
        public Matrix4 Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
            }
        }
        public Vector2 endNode
        {
            get
            {
                return EndNode;
            }
            set
            {
                EndNode = value;
            }
        }

        public bool chaseFinish
        {
            get
            {
                return ChaseFinish;
            }
            set
            {
                ChaseFinish = value;
            }
        }
        public T getComponent<T>() where T : IComponent
        {
            foreach (IComponent t in componentList)
            {
                if (t.GetType() == typeof(T))
                {
                    return (T)t;
                }
            }
            return default;
        }
    }
}
