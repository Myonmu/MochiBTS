using System;
using DG.Tweening;
namespace MochiBTS.Extension.Primitives
{
    public interface ITweener
    {
        public Tweener tweener { get; set; }
        public Action<Tweener> decoratorCallback { get; set; }
    }
}