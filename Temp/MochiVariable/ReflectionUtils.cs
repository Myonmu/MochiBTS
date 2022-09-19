using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
namespace DefaultNamespace.MochiVariable
{
    /// <summary>
    /// Dark magic. Not friend of IL2CPP (Source generator required if it is the case)
    /// </summary>
    public static class ReflectionUtils
    {
        public static Func<T> CreateGetter<T>(Component selectedComponent, FieldInfo field)
        {
            try {
                var expr = Expression.Field(field.IsStatic ? null : Expression.Constant(selectedComponent), field);
                return Expression.Lambda<Func<T>>(expr).Compile();
            } catch (Exception e) {
                return null;
            }
        }
        public static Action<T> CreateSetter<T>(Component selectedComponent, FieldInfo field)
        {

            try {
                var param = Expression.Parameter(typeof(T));
                var expr = Expression.Field(field.IsStatic ? null : Expression.Constant(selectedComponent), field);
                var assign = Expression.Assign(expr, param);
                return Expression.Lambda<Action<T>>(assign, param).Compile();
            } catch (Exception e) {
                return null;
            }
        }
        public static Func<T> CreateNestedGetter<T>(Component selectedComponent, PropertyInfo property, PropertyInfo second)
        {
            try {
                var expr = Expression.Property(Expression.Constant(selectedComponent), property);
                expr = Expression.Property(expr, second);
                return Expression.Lambda<Func<T>>(expr).Compile();
            } catch (Exception e) {
                return null;
            }
        }
        public static Action<T> CreateNestedSetter<T>(Component selectedComponent, PropertyInfo property, PropertyInfo second)
        {
            try {
                var param = Expression.Parameter(typeof(T));
                var expr = Expression.Property(Expression.Constant(selectedComponent), property);
                expr = Expression.Property(expr, second);
                var assign = Expression.Assign(expr, param);
                return Expression.Lambda<Action<T>>(assign, param).Compile();
            } catch (Exception e) {
                return null;
            }
        }
        public static Func<T> CreateNestedGetter<T>(Component selectedComponent, FieldInfo field, PropertyInfo second)
        {
            try {
                var expr = Expression.Field(field.IsStatic?null:Expression.Constant(selectedComponent), field);
                expr = Expression.Property(expr, second);
                return Expression.Lambda<Func<T>>(expr).Compile();
            } catch (Exception e) {
                return null;
            }
        }
        public static Action<T> CreateNestedSetter<T>(Component selectedComponent, FieldInfo field, PropertyInfo second)
        {
            try {
                var param = Expression.Parameter(typeof(T));
                var expr = Expression.Field(field.IsStatic?null:Expression.Constant(selectedComponent), field);
                expr = Expression.Property(expr, second);
                var assign = Expression.Assign(expr, param);
                return Expression.Lambda<Action<T>>(assign, param).Compile();
            } catch (Exception e) {
                return null;
            }
        }
    }
}