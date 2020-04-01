using System;
using Xunit;

namespace Project1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            System.Threading.Thread.Sleep(1500);
            // without failure it will output 
            //             Stack Trace:
            //     at Project2.UnitTest1.Test3() in /mnt/c/Projects/temp/TestTerminalIssues/Project2/UnitTest1.cs:line 31

            // Test Run Successful.
            // Total tests: 1
            //     Passed: 1
            // Total time: 3.2791 Seconds
            // ^[[33;1R
            //
            // with failure it will lock down
            //Assert.Equal(1, 2);
        }
    }
}
