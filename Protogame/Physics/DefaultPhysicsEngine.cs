using System;
using System.Collections.Generic;
using System.Linq;
using Jitter;
using Jitter.Dynamics;
using Protoinject;

namespace Protogame
{
    public class DefaultPhysicsEngine : IPhysicsEngine
    {
        private readonly IPhysicsFactory _physicsFactory;
        private IWorld _currentWorld;
        private PhysicsShadowWorld _shadowWorld;

        public DefaultPhysicsEngine(IPhysicsFactory physicsFactory)
        {
            _physicsFactory = physicsFactory;
        }
        
        public void Update(IGameContext gameContext, IUpdateContext updateContext)
        {
            if (gameContext.World != _currentWorld || _shadowWorld == null)
            {
                if (_shadowWorld != null)
                {
                    _shadowWorld.Dispose();
                }

                _shadowWorld = _physicsFactory.CreateShadowWorld(gameContext.World);
                _currentWorld = gameContext.World;
            }

            _shadowWorld.Update(gameContext, updateContext);
        }

        public void RegisterRigidBodyForHasMatrixInCurrentWorld(RigidBody rigidBody, IHasMatrix hasMatrix)
        {
            _shadowWorld.RegisterRigidBodyForHasMatrix(rigidBody, hasMatrix);
        }

        public void DebugRender(IGameContext gameContext, IRenderContext renderContext)
        {
            _shadowWorld.Render(gameContext, renderContext);
        }
    }
}