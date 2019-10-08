using System;
using System.Collections.Generic;
using System.Linq;

namespace RichTextBlock.Control
{
    internal class FormatFrameArgs : EventArgs, IDisposable
    {
        public FormatFrameArgs(IList<TextFormatCache> formatCache)
        {
            TextFrames = formatCache.ToArray().ToList();
        }

        public List<TextFormatCache> TextFrames { get; private set; }

        #region IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            TextFrames.Clear();
            TextFrames = null;
        }

        #endregion

        public FormatFrameArgs MemClone()
        {
            return new FormatFrameArgs(TextFrames);
        }
    }
}