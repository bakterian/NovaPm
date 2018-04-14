using NovaPm.Control.Services;
using NovaPm.Infrastructure.Interfaces;
using NSubstitute;
using System;
using Xunit;

namespace NovaPmData.ControlTests
{
    public class MeasChunkCreatorTest
    {
        private readonly IParameterStore _parameterStoreDummy;

        public MeasChunkCreatorTest()
        {
            _parameterStoreDummy = Substitute.For<IParameterStore>();

            _parameterStoreDummy.ChunkSize = 20;
            _parameterStoreDummy.ChunkTimeInterval = 1;
            _parameterStoreDummy.MessageHeaderControlIdStr = "AAC0";
            _parameterStoreDummy.MsgTail = "AB";
        }

        [Fact]
        public void ShouldCreateMeasDataChunkFromValidStr()
        {
            var testInput = "AAC058008300A96EF2AB";
            var measChunkCreator = new MeasChunkCreator(_parameterStoreDummy);

            var measChunk = measChunkCreator.CreateMeasChunk(testInput,DateTime.MinValue);

            Assert.NotNull(measChunk);
            Assert.True(8 - measChunk.Pm2_5Amount < 0.00001);
            Assert.True(13 - measChunk.Pm10Amount < 0.00001);
            Assert.Equal(0x6EA9, measChunk.ID);
            Assert.Equal(0xF2, measChunk.Checksum);
        }



        [Theory]
        [InlineData("AAC058008300A96EF2AB", true)]
        [InlineData("AAC058008300A96EFFAB", false)]
        [InlineData("AAC059007C00A96EECAB", true)]      
        [InlineData("AAC058008300J96EF2AB", false)]
        [InlineData("AAC058B",              false)]
        [InlineData(" AC058008300A96EF2AB", false)]
        [InlineData("AAC058008300A96EF2BB", false)]
        [InlineData("AAC058008300A96EF2B%", false)]
        [InlineData("AAC05A007D00A96EEEAB", true)]
        public void ShouldProperlyValidateInputsStrings(string testInput, bool expectedResult)
        {
            var measChunkCreator = new MeasChunkCreator(_parameterStoreDummy);

            var actualValidationResult = measChunkCreator.IsValidStrChunk(testInput);

            Assert.Equal(expectedResult, actualValidationResult);
        }
    }
}
