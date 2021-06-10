using System;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    enum ComponentTypes {
        COMPONENT_NONE     = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_AUDIO    = 1 << 2,
        COMPONENT_VELOCITY = 1 << 3,
        COMPONENT_COLLISIONSPHERE = 1 << 4,
        COMPONENT_COLLISIONLINE = 1 << 5,
        COMPONENT_PLAYER = 1 << 6,
        COMPONENT_AI = 1 << 7
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
        void Close();
    }
}
