using System;
using DG.Tweening;
namespace MochiBTS.Extension.Primitives
{
    public interface ITweener
    {
        public Tweener Tweener { get; set; }
        public Action<Tweener> DecoratorCallback { get; set; }
    }
}