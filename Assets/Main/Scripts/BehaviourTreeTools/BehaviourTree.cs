using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeNS
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        public Node root { get; private set; } = null;

        ///<summary>Holds the interval between changing current state</summary>
        [SerializeField]
        protected float interval = 1 / 40;

        ///<summary>Tells whether tree should use coroutines or updates for running the logic.
        ///true means coroutines, false is for updates</summary>
        [SerializeField]
        bool coroutineSwitch = false;

        protected virtual void Start()
        {
            root = SetupTree();
            if (coroutineSwitch)
            {
                StartCoroutine(RunTree());
            }
        }

        protected virtual void Update()
        {
            if (!coroutineSwitch && root != null)
            {
                root.Run();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (!coroutineSwitch && root != null)
            {
                root.FixedRun();
            }
        }

        private IEnumerator RunTree()
        {
            if (root != null)
            {
                root.Run();
                yield return new WaitForSeconds(interval);
            }
            else
            {
                yield break;
            }
        }

        protected abstract Node SetupTree();
    }
}
