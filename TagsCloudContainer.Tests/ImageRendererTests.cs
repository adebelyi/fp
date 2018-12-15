﻿using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudContainer.ResultRenderer;

namespace TagsCloudContainer.Tests
{
    [TestFixture]
    public class ImageRendererTests
    {
        [TestCase(0, 0)]
        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(-1, -1)]
        public void Generate_ReturnsFail_OnInvalidSize(int width, int height)
        {
            var renderer = new ImageRenderer(new Config {ImageSize = new Size(width, height)});
            var expected = Result.Fail<Image>("Width and height of image have to be > 0");

            renderer.Generate(Enumerable.Empty<Word>())
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Generate_ThrowsArgumentException_OnNullWords()
        {
            Action action = () => new ImageRenderer(new Config {ImageSize = new Size(1024, 1024)}).Generate(null);

            action
                .Should()
                .Throw<ArgumentException>();
        }

        [TestCase(10, 10)]
        [TestCase(800, 600)]
        [TestCase(1024, 1024)]
        public void Generate_GeneratesImageWithGivenSize(int width, int height)
        {
            var expectedSize = new Size(width, height);

            var renderer = new ImageRenderer(new Config {ImageSize = expectedSize});

            renderer
                .Generate(Enumerable.Empty<Word>())
                .GetValueOrThrow()
                .Size.Should()
                .BeEquivalentTo(expectedSize);
        }
    }
}