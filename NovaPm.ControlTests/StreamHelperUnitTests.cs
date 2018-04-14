
using NovaPm.Control.Services;
using NovaPm.Infrastructure.Interfaces;
using NSubstitute;
using System;
using Xunit;

namespace NovaPm.ControlTests
{ 
	public class StreamHelperUnitTests
    {
        private IParameterStore _parameterStoreDummy;

        public StreamHelperUnitTests()
        {
            _parameterStoreDummy = Substitute.For<IParameterStore>();
        }

        [Theory]
        [InlineData("szyna_kolejowa======BINGO============", "BINGO", 20)]
        [InlineData("LOREM_IPSUM_LOREMLOREMIPSUMaac0LOREMIPUSMBINGO", "BINGO", 41)]
        [InlineData("BINGOLOREM_IPSUM_LOREMLOREMIPSUMAAC0LOREMIPUSM", "BINGO", 0)]
        [InlineData("LOREM_IPSUM_LOREMLOREMIPSUMAAC0LOREMIPUSM", "AAC0", 27)]
        [InlineData("blabla_sflkfsdj_vlkfj_DSFDFDSAAC0GFDGDF","AAC0",29)]
        public void FindsMsgHeaderIndexAccuretly(string capturedData, string MsgHeader,int expectedIndex)
        {
            _parameterStoreDummy.MessageHeaderControlIdStr = MsgHeader;

            var streamHelper = new StreamHelper(_parameterStoreDummy);

            var actualIndex = streamHelper.GetMsgHeaderStartIndex(capturedData);

            Assert.Equal(expectedIndex, actualIndex);
        }

        [Fact]
        public void ShouldRemoveUnwantedChars()
        {
            var dummyCaptureData = "gfdgfd fsdf dffds \r\n \nfdsf fds";

            var streamHelper = new StreamHelper(_parameterStoreDummy);

            var cleanedCaptureData = streamHelper.RemoveUnwantedChars(dummyCaptureData);

            Assert.DoesNotContain("\r", cleanedCaptureData);
            Assert.DoesNotContain("\n", cleanedCaptureData);
            Assert.DoesNotContain(" ", cleanedCaptureData);
            Assert.Equal(22, cleanedCaptureData.Length);
        }

        [Theory]
        [InlineData("BLABLA\r\nTerminal log file\r\nDate: 2018-04-14 - 14:22:03gfdgdfgfdf\r\n",true)]
        [InlineData("BLABLA\r\nTerminal log file\nDate: 2018-04-14 - 14:22:03gfdgdfgfdf\r\n", true)]
        [InlineData("BLABLA\r\nTerminal log file\r\nDae: 2018-04-14 - 14:22:03gfdgdfgfdf\r\n", false)]
        [InlineData("BLABLA\r\nTerminal log file\r\nDae:gfdgdfgfdf\r\n", false)]
        [InlineData("Terminal log file\r\nDate: 2018-04-14 - 14:22:03", true)]
        public void ShouldRecognizedStartTimeDefinitionInTerminalCapturedLog(string capturedData,bool expectedSearchResult)
        {
            _parameterStoreDummy.StartTimeStringNowNewLines = "Terminal log fileDate: ";
            _parameterStoreDummy.StartTimeInfoLength = 44;

            var streamHelper = new StreamHelper(_parameterStoreDummy);

            var containsStartTimeDef = streamHelper.ContainsStartTime(capturedData);

            Assert.Equal(expectedSearchResult, containsStartTimeDef);
        }

        [Fact]
        public void ShouldRetrieveStartTime()
        {
            _parameterStoreDummy.StartTimeStringNowNewLines = "Terminal log fileDate: ";
            _parameterStoreDummy.StartTimeInfoLength = 44;
            var dummyCaptureData = "Terminal log file\r\nDate: 2018-04-16 - 12:22:03";

            var streamHelper = new StreamHelper(_parameterStoreDummy);

            var expectedStartTime = new DateTime(2018, 4, 16, 12, 22, 3);
            var parsedStartTime = streamHelper.GetStartTime(dummyCaptureData);

            Assert.Equal(expectedStartTime, parsedStartTime);
        }


    }
}