﻿using System;
using System.Collections.Generic;
using System.Text;
using AstUtils = Microsoft.Scripting.Ast.Utils;
using Et = System.Linq.Expressions.Expression;

namespace IronJS.Compiler.Ast
{
    public enum WhileType { DoWhile, While }

    public class WhileNode : LoopNode
    {
        public Node Test { get; protected set; }
        public Node Body { get; protected set; }
        public WhileType Loop { get; protected set; }

        public WhileNode(Node test, Node body, WhileType type)
            : base(NodeType.While)
        {
            Test = test;
            Body = body;
            Loop = type;
        }

        public override void Print(StringBuilder writer, int indent = 0)
        {
            var indentStr = new String(' ', indent * 2);

            writer.AppendLine(indentStr + "(" + Loop);

            Test.Print(writer, indent + 1);
            Body.Print(writer, indent + 1);

            writer.AppendLine(indentStr + ")");
        }

        public override Et LoopWalk(EtGenerator etgen)
        {
            Et loop = null;

            var test = Et.Dynamic(
                etgen.Context.CreateConvertBinder(typeof(bool)),
                typeof(bool),
                Test.Walk(etgen)
            );

            // while
            if (Loop == Ast.WhileType.While)
            {
                var body = Body.Walk(etgen);

                loop = AstUtils.While(
                    test,
                    body,
                    null,
                    etgen.FunctionScope.LabelScope.Break(),
                    etgen.FunctionScope.LabelScope.Continue()
                );
            }
            // do ... while
            else if (Loop == Ast.WhileType.DoWhile)
            {
                var bodyExprs = new List<Et>();

                bodyExprs.Add(Body.Walk(etgen));

                // test last, instead of first
                bodyExprs.Add(
                    Et.IfThenElse(
                        test,
                        Et.Continue(etgen.FunctionScope.LabelScope.Continue()),
                        Et.Break(etgen.FunctionScope.LabelScope.Break())
                    )
                );

                loop = Et.Loop(
                    Et.Block(bodyExprs),
                    etgen.FunctionScope.LabelScope.Break(),
                    etgen.FunctionScope.LabelScope.Continue()
                );
            }

            return loop;
        }
    }
}
