using System;
using Gallio.Common.Reflection;
using Gallio.Framework;
using Gallio.Framework.Pattern;
using Gallio.Model;
using MbUnit.Framework;
using ProtoTest.Golem.Proxy.HAR;

namespace ProtoTest.Golem.Core
{
    /// <summary>
    /// Re run a test up to X times should it fail
    /// </summary>
    [AttributeUsage(PatternAttributeTargets.Test, AllowMultiple = true, Inherited = true)]
    public class RepeatOnFailureAttribute : TestDecoratorPatternAttribute
    {
        private readonly int _maxNumberOfAttempts;

        /// <summary>
        ///     Will re-run the test method each time we get a failure for a limited number of attempts.
        /// </summary>
        /// <example>
        ///     <code><![CDATA[
        /// [Test]
        /// [RepeatOnFailure(3)]
        /// public void Test()
        /// {
        ///     // This test will be executed until we get a pass or have run it 3 times.
        ///     // Eg, if the first test run fails, we will run it again, and if the second attempt passes, then we will stop.
        ///     // if 3 attempts all fail, we dont try anymore
        /// }
        /// ]]></code>
        /// </example>
        /// <param name="maxNumberOfAttempts">The number of times to repeat the test while searching for a pass</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown if <paramref name="maxNumberOfAttempts" />
        ///     is less than 1.
        /// </exception>
        public RepeatOnFailureAttribute(int maxNumberOfAttempts)
        {
            if (maxNumberOfAttempts < 1)
                throw new ArgumentOutOfRangeException("maxNumberOfAttempts",
                    @"The maximum number of attempts must be at least 1.");

            _maxNumberOfAttempts = maxNumberOfAttempts;
        }

        /// <inheritdoc />
        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            scope.TestBuilder.TestInstanceActions.RunTestInstanceBodyChain.Around(
                delegate(PatternTestInstanceState state,
                    Gallio.Common.Func<PatternTestInstanceState, TestOutcome> inner)
                {
                    TestOutcome outcome = TestOutcome.Passed;
                    int failureCount = 0;
                    // we will try up to 'max' times to get a pass, if we do, then break out and don't run the test anymore
                    for (int i = 0; i < _maxNumberOfAttempts; i++)
                    {
                        string name = String.Format("Repetition #{0}", i + 1);
                        TestContext context = TestStep.RunStep(name, delegate
                        {
                            TestOutcome innerOutcome = inner(state);
                            // if we get a fail, and we have used up the number of attempts allowed to get a pass, throw an error
                            if (innerOutcome.Status != TestStatus.Passed)
                            {
                                throw new SilentTestException(innerOutcome);
                            }
                        }, null, false, codeElement);

                        outcome = context.Outcome;
                        // escape the loop if the test has passed, otherwise increment the failure count
                        if (context.Outcome.Status == TestStatus.Passed)
                            break;
                        failureCount++;
                    }

                    //TestLog.WriteLine(String.Format(
                    //Log.Message(String.Format(
                    //    failureCount == _maxNumberOfAttempts
                    //        ? "Tried {0} times to get a pass test result but didn't get it"
                    //        : "The test passed on attempt {1} out of {0}", _maxNumberOfAttempts, failureCount + 1));

                    return outcome;
                });
        }
    }
}