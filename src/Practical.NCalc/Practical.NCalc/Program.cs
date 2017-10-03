using NCalc;
using System;
using System.Linq;

namespace Practical.NCalc
{
    class Program
    {
        static void Main(string[] argsx)
        {
            Expression e = new Expression("if (And(NOT(ISBLANK(ReceivedDate)), ReceivedDate >= Date(2017,01,01), ReceivedDate <= Date(2018,12,31)), Total * 0.1, Total * VAT)");

            e.Parameters["ReceivedDate"] = DateTime.Parse("2018/01/01");
            e.Parameters["VAT"] = (decimal)12 / 100;
            e.Parameters["Total"] = 1000;

            e.EvaluateFunction += delegate (string name, FunctionArgs args)
            {
                var operatorName = name.ToUpper();
                if (operatorName == "DATE")
                {
                    args.Result = new DateTime((int)args.Parameters[0].Evaluate(), (int)args.Parameters[1].Evaluate(), (int)args.Parameters[2].Evaluate());
                }
                if (operatorName == "AND")
                {
                    args.Result = args.Parameters.All(x => (bool)x.Evaluate());
                }
                if (operatorName == "ISBLANK")
                {
                    var temp = args.Parameters[0].Evaluate();
                    args.Result = temp == null || string.IsNullOrWhiteSpace(temp.ToString());
                }
                if (operatorName == "NOT")
                {
                    args.Result = !(bool)args.Parameters[0].Evaluate();
                }
            };

            var rs = e.Evaluate();
        }
    }
}
