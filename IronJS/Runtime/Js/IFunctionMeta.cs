﻿using System.Dynamic;
using IronJS.Runtime.Utils;
using Microsoft.Scripting.Utils;
using AstUtils = Microsoft.Scripting.Ast.Utils;
using Et = System.Linq.Expressions.Expression;
using Meta = System.Dynamic.DynamicMetaObject;

namespace IronJS.Runtime.Js
{
    class IFunctionMeta : IObjMeta
    {
        public IFunctionMeta(Et parameter, IFunction function)
            : base(parameter, function)
        {

        }

        public override Meta BindInvoke(InvokeBinder binder, Meta[] args)
        {
            return new Meta(
                Et.Call(
                    EtUtils.Cast<IFunction>(
                        this.Expression
                    ),
                    IFunctionMethods.MiCall,
                    EtUtils.Cast<IObj>(
                        args[0].Expression
                    ),
                    AstUtils.NewArrayHelper(
                        typeof(object),
                        DynamicUtils.GetExpressions(
                            ArrayUtils.RemoveFirst(args)
                        )
                    )
                ),
                RestrictUtils.BuildCallRestrictions(
                    this,
                    args,
                    RestrictFlag.Type
                )
            );
        }
    }
}
