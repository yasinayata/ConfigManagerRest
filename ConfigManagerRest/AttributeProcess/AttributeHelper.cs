using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ConfigManagerRest.AttributeProcess
{
    #region AttributeHelper
    public static class AttributeHelper
    {
        public static TValue GetPropertyAttributeValue<T, TOut, TAttribute, TValue>
            (Expression<Func<T, TOut>> propertyExpression,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            try
            {
                var expression = (MemberExpression)propertyExpression.Body;
                var propertyInfo = (PropertyInfo)expression.Member;
                var attr = propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
                return attr != null ? valueSelector(attr) : default(TValue);

            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
                return default(TValue);
            }
        }
    }
    #endregion
}