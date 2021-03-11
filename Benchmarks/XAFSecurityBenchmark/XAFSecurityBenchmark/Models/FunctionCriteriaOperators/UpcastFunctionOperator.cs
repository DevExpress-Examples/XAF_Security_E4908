using System;
using System.Linq.Expressions;
using System.Reflection;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace XAFSecurityBenchmark.Models.FunctionCriteriaOperators {
    public class UpcastFunctionOperator : ICustomFunctionOperatorConvertibleToExpression {
        public const string OperatorName = "Upcast";

        private static readonly UpcastFunctionOperator instance = new UpcastFunctionOperator();
        public static void Register() {
            CustomFunctionOperatorHelper.Register(instance);
        }

        static Type GetTypeByName(string typeName) => XafTypesInfo.Instance.FindTypeInfo(typeName).Type;
        string ICustomFunctionOperator.Name => OperatorName;

        Expression ICustomFunctionOperatorConvertibleToExpression.Convert(ICriteriaToExpressionConverter converter, params Expression[] operands) {
            ConstantExpression typeNameExpr = operands[1] as ConstantExpression;
            ConstantExpression propertyNameExpr = operands[2] as ConstantExpression;
            if(typeNameExpr == null) {
                throw new ArgumentException("The Upcast function expects a constant as the second argument (type name).");
            }
            if(propertyNameExpr == null) {
                throw new ArgumentException("The Upcast function expects a constant as the third argument (property name).");
            }
            string typeName = typeNameExpr.Value as string;
            string propertyName = propertyNameExpr.Value as string;
            if(string.IsNullOrWhiteSpace(typeName)) {
                throw new ArgumentException("The type name parameter is not a string value or an empty string");
            }
            if(string.IsNullOrWhiteSpace(propertyName)) {
                throw new ArgumentException("The property name parameter is not a string value or an empty string");
            }
            Type type = GetTypeByName(typeName);
            return Expression.Property(
                Expression.Convert(operands[0], type),
                propertyName
                );
        }

        object ICustomFunctionOperator.Evaluate(params object[] operands) {
            if(operands[0] != null) {
                string typeName = operands[1] as string;
                string propertyName = operands[2] as string;
                if(string.IsNullOrWhiteSpace(typeName)) {
                    throw new ArgumentException("The type name parameter is not a string value or an empty string");
                }
                if(string.IsNullOrWhiteSpace(propertyName)) {
                    throw new ArgumentException("The property name parameter is not a string value or an empty string");
                }
                Type type = GetTypeByName(typeName);
                return type.InvokeMember(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, operands[0], new object[0]);
            }
            return null;
        }

        Type ICustomFunctionOperator.ResultType(params Type[] operands) => typeof(object);
    }
}
