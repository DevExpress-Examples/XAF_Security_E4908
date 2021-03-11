using System;
using System.Linq.Expressions;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using XAFSecurityBenchmark.Models.Base;

namespace XAFSecurityBenchmark.Models.FunctionCriteriaOperators {
    public class CurrentUserDepartmentOperator : ICustomFunctionOperatorConvertibleToExpression {
        public const string OperatorName = "CurrentUserDepartment";
        private static readonly CurrentUserDepartmentOperator instance = new CurrentUserDepartmentOperator();
        public static void Register() {
            CustomFunctionOperatorHelper.Register(instance);
        }
        public object Evaluate(params object[] operands) {
            object currentUserDepartment = ((ICustomPermissionPolicyUser)SecuritySystem.CurrentUser).Department;
            return currentUserDepartment;
        }
        public string Name {
            get { return OperatorName; }
        }
        public Type ResultType(params Type[] operands) {
            return typeof(object);
        }

        Expression ICustomFunctionOperatorConvertibleToExpression.Convert(ICriteriaToExpressionConverter converter, params Expression[] operands) {
            object currentUserDepartment = ((ICustomPermissionPolicyUser)SecuritySystem.CurrentUser).Department;
            return Expression.Constant(currentUserDepartment);
        }
    }
}
