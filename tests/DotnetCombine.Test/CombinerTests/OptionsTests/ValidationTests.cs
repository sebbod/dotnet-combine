﻿using DotnetCombine.Options;
using DotnetCombine.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DotnetCombine.Test.CombinerTests.OptionsTests
{

    public class ValidationTests : BaseCombinerTests
    {
        [Fact]
        public async Task NoInput_ShouldFail()
        {
            // Arrange
            var options = new CombineOptions();

            Assert.ThrowsAsync<ArgumentException>(async () => { await new Combiner(options).Run(); });
        }

        [Fact]
        public async Task NonExistingDirInput_ShouldFail()
        {
            // Arrange
            var options = new CombineOptions()
            {
                Input = "./___non_existing_dir___/"
            };

            // Act and assert
            Assert.ThrowsAsync<ArgumentException>(async () => { await new Combiner(options).Run(); });
        }

        [Fact]
        public async Task NonExistingFileInput_ShouldFail()
        {
            // Arrange
            var options = new CombineOptions()
            {
                Input = "./___non_existing_file___"
            };

            // Act and assert
            Assert.ThrowsAsync<ArgumentException>(async () => { await new Combiner(options).Run(); });
        }
    }
}