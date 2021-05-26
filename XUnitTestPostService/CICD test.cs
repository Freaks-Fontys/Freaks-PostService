using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestPostService
{
    public class CICD_test
    {
        [Fact]
        public void BasicTest()
        {
            var expected = 1;
            var actual = 1;

            Assert.Equal(expected, actual);
        }
    }
}
