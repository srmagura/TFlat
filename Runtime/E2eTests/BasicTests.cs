using TFlat.Compiler;
using TFlat.Runtime;

namespace E2eTests;

[TestClass]
public class BasicTests
{
    private static async Task<string> CompileAndRunAsync(string code)
    {
        using var dllStream = new MemoryStream();
        await CompilerProgram.CompileModuleAsync(code, dllStream);

        dllStream.Position = 0;
        var ast = await RuntimeProgram.LoadAssemblyAsync(dllStream);

        using var standardOut = new StringWriter();

        var executionOptions = new ExecutionOptions
        {
            StandardOut = standardOut
        };

        RuntimeProgram.RunAssembly(ast, executionOptions);

        var sb = standardOut.GetStringBuilder().Replace("\r\n", "\n");

        Assert.AreEqual(sb[^1], '\n');
        return sb.Remove(sb.Length - 1, 1).ToString();
    }

    [TestMethod]
    public async Task HelloWorld()
    {
        var output = await CompileAndRunAsync(CodeFixtures.HelloWorld);
        Assert.AreEqual("hello world", output);
    }

    [TestMethod]
    public async Task MultipleFunctions()
    {
        var output = await CompileAndRunAsync(CodeFixtures.MultipleFunctions);
        Assert.AreEqual("3\n.14159", output);
    }

    [TestMethod]
    public async Task ConstVariable()
    {
        var output = await CompileAndRunAsync(CodeFixtures.ConstVariable);
        Assert.AreEqual("apple", output);
    }

    [TestMethod]
    public async Task LetVariable()
    {
        var output = await CompileAndRunAsync(CodeFixtures.LetVariable);
        Assert.AreEqual("7\n3", output);
    }

    [TestMethod]
    public async Task Math()
    {
        var code = @"
export fun main(): void {
    print((-3 + 1) * 7 \\ 2 - 1);
    print((10 + 10) / 2**3);
    print(1 - 17 % 2);
}
        ";

        var output = await CompileAndRunAsync(code);
        Assert.AreEqual("-8\n2.5\n0", output);
    }
}
