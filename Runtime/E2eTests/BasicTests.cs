using System.Text;
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

        return standardOut.GetStringBuilder()
            .Replace("\r\n", "\n")
            .ToString();
    }

    [TestMethod]
    public async Task HelloWorld()
    {
        var output = await CompileAndRunAsync(CodeFixtures.HelloWorld);
        Assert.AreEqual("hello world\n", output);
    }
}
