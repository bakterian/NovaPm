using NovaPm.Control.Services;
using System.IO;
using Xunit;

namespace NovaPm.ControlTests
{
    public class UserInputManagerUnitTests
    {
        [Theory]
        [InlineData(new string[] { "testFilePath" }, true)]
        [InlineData(new string[] { "testFilePath", "secondArg" }, false)]
        [InlineData(new string[] { }, false)]
        public void ShouldRecoginzeInputArgument(string[] args,bool expectedResult)
        {
            var userInputManager = new UserInputManager();

            var evalResult = userInputManager.LogFileWasProvided(args);

            Assert.Equal(expectedResult, evalResult);
        }

        [Fact]
        public void ShouldNoticeThatLogFileExists()
        {
            var testFilePath = $"{Path.GetTempPath()}\\testFile.txt";

            if (!File.Exists(testFilePath)) File.Create(testFilePath).Close();

            var userInputManager = new UserInputManager();

            var evalResult = userInputManager.LogFileExists(testFilePath);

            Assert.True(evalResult);
        }

        [Fact]
        public void ShouldNoticeThatLogFileExistsWhenProvidingOnlyTheName()
        {
            var testFilePath = $"{Directory.GetCurrentDirectory()}\\testFileInCurrentDirectory.txt";

            if (!File.Exists(testFilePath)) File.Create(testFilePath).Close();

            var userInputManager = new UserInputManager();

            var evalResult = userInputManager.LogFileExists("testFileInCurrentDirectory.txt");

            Assert.True(evalResult);
        }

        [Fact]
        public void ShouldNoticeThatLogFileDoesNotExists()
        {
            var testFilePath = $"{Path.GetTempPath()}\\testFileNonExistent.txt";

            var userInputManager = new UserInputManager();

            var evalResult = userInputManager.LogFileExists(testFilePath);

            Assert.False(evalResult);
        }

    }
}
