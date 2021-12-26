namespace TestUtil;

public static class CodeFixtures
{
    public const string HelloWorld = @"
fun main(): void {
    print(""hello world"");
}
        ";

    public const string MultipleFunctions = @"
fun print3(): void {
    print(3);
}

fun main(): void {
    print3();
    print("".14159"");
}
        ";

    public const string ConstVariable = @"
fun main(): void {
    const a: string = ""apple"";
    print(a);
}
        ";

    public const string LetVariable = @"
fun main(): void {
    let my_variable: int = 7;
    print(my_variable);

    my_variable = 3;
    print(my_variable);
}
        ";

    public const string MathOperators = @"
fun main(): void {
    print(-3);
    print(1 + 2);
    print(1 - 2);
    print(1 * 2);
    print(1 \\ 2);
    print(2 \\ 2);
    print(2**3);
}
    ";
}
