using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetPath
{
    public class Mouse_Select : MonoBehaviour
    {

        public Coroutine coroutine { get; private set; }
        public object result;
        private IEnumerator target;

        public Mouse_Select(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }

        
    }
}
