using System;

namespace JetBrains.ReSharper.SamplePlugin.Tests.TestExamples
{
    public class ThrowExamples
    {
        /// <summary>
        /// Throw1
        /// </summary>
        /// <exception cref="ArgumentException">TST1</exception>
        public void Throw1(int x)
        {
            if (x == 2)
                throw new InvalidOperationException("11"); // W
            if (x == 2)
                throw new ArgumentException("TST1");    // OK
            if (x == 2)
                throw new ArgumentException("TST2");    // H
            if (x == 2)
                throw new ArgumentException();          // H
        }
    }
}