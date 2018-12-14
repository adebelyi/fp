﻿using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer.WordsPreprocessors
{
    public class CustomBoringWordsRemover : IWordsPreprocessor
    {
        private readonly HashSet<string> customBoringWords;

        public CustomBoringWordsRemover(ICustomWordsRemoverConfig config)
        {
            customBoringWords = new HashSet<string>(config.CustomBoringWords);
        }

        IEnumerable<string> IWordsPreprocessor.Preprocess(IEnumerable<string> words)
        {
            return words.Where(word => !customBoringWords.Contains(word));
        }
    }
}