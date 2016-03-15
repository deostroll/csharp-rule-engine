using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Workflow.Activities.Rules;
using System.CodeDom;

namespace caRuleEngineSample
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvReader rdr = new CsvReader(new StreamReader("data\\data.csv"));
            var records = new List<LineItem>(rdr.GetRecords<LineItem>());
            
            foreach (var item in records)
            {
                Console.WriteLine(item.ItemName);
            }

            RuleSet rs = new RuleSet("ApplyOffer");
            rs.Rules.Add(new Rule(
                name: "ApplyDiscount",
                condition: new RuleExpressionCondition(
                    expression: new CodeBinaryOperatorExpression(
                        left: new CodeBinaryOperatorExpression(
                            left: new CodePropertyReferenceExpression(
                                targetObject: new CodeThisReferenceExpression(),
                                propertyName: "Category"),
                            op: CodeBinaryOperatorType.ValueEquality,
                            right: new CodePrimitiveExpression("A")
                            ),
                        op: CodeBinaryOperatorType.BooleanAnd,
                        right: new CodeBinaryOperatorExpression(
                            left: new CodePropertyReferenceExpression(
                                targetObject: new CodeThisReferenceExpression(),
                                propertyName: "DiscountOption"),
                            op: CodeBinaryOperatorType.ValueEquality,
                            right: new CodePrimitiveExpression("10PCOFF")
                            )
                    )
                ),
                thenActions: new List<RuleAction>()
                    {
                        new RuleStatementAction(
                            new CodeAssignStatement(
                                left: new CodePropertyReferenceExpression(
                                    targetObject: new CodeThisReferenceExpression(),
                                    propertyName: "Remarks"),
                                right: new CodePrimitiveExpression("10 % Discount Applied"))
                                )
                    }
                )
                
            );
                        
            RuleEngine engine = new RuleEngine(rs, typeof(LineItem));
            foreach (var item in records)
            {
                engine.Execute(item);
                Console.WriteLine("Sno: {0}, Item: {1}, Remark: {2}", item.Sno, item.ItemName, item.Remarks);
            }

            CsvWriter writer = new CsvWriter(new StreamWriter("output.csv"));
            writer.WriteRecords(records);
            
        }
    }
}
