﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudContainer.ResultRenderer;
using TagsCloudContainer.WordFormatters;
using TagsCloudContainer.WordLayouts;
using TagsCloudContainer.WordsPreprocessors;

namespace TagsCloudContainer
{
    public class TagsCloudBuilder
    {
        private readonly IEnumerable<IWordsPreprocessor> wordsPreprocessors;
        private readonly IWordFormatter wordFormatter;
        private readonly ILayouter layouter;
        private readonly IResultRenderer resultRenderer;
        private readonly WordsSizer wordsSizer;

        public TagsCloudBuilder(
            IEnumerable<IWordsPreprocessor> wordsPreprocessors,
            IWordFormatter wordFormatter,
            ILayouter layouter,
            IResultRenderer resultRenderer,
            WordsSizer wordsSizer)
        {
            this.wordsPreprocessors = wordsPreprocessors;
            this.wordFormatter = wordFormatter;
            this.layouter = layouter;
            this.resultRenderer = resultRenderer;
            this.wordsSizer = wordsSizer;
        }

        public Result<Image> Visualize(IEnumerable<string> rawWords)
        {
            return Result.Of(() => wordsPreprocessors.Aggregate(rawWords,
                    (current, preprocessor) => preprocessor.Preprocess(current)))
                .Then(preprocessedWords => wordFormatter.FormatWords(preprocessedWords))
                .Then(formattedWords => formattedWords.Select(word => new Word(word.Font, word.Color, word.Value)
                {
                    Position = layouter
                        .GetNextPosition(wordsSizer.GetWordSize(word))
                }))
                .Then(positionedWords => resultRenderer.Generate(positionedWords));
        }
    }
}