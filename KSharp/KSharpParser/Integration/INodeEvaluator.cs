namespace KSharpParser
{
    /// <summary>
    /// Defines which resolve methods need to be implemented for visitor to work properly.
    /// </summary>
    public interface INodeEvaluator
    {
        object EvaluateMethod(object methodName);

        object EvaluateCondition();

        object EvaluateLambdaExpr();

        object EvaluateWhileLoop();

        object EvaluateForLoop();

        object EvaluateForEachLoop();

        object EvaluateIsNullOperator();

        object EvaluateAssignment();

        object EvaluateTernaryOperator();

        object EvaluateEquality();

        object EvaluateMethodCall();
    }
}
 