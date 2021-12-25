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
}
